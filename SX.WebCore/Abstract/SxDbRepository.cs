using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SX.WebCore.Abstract
{
    public abstract class SxDbRepository<TKey, TModel, TDbContext>
        where TModel : SxDbModel<TKey>
        where TDbContext : SxDbContext
    {
        private SxDbContext _dbContext;
        private string _connectionString;
        public SxDbRepository()
        {
            _dbContext = Activator.CreateInstance<TDbContext>();
            _connectionString = _dbContext.Database.Connection.ConnectionString;
        }
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public virtual TModel Create(TModel model)
        {
            if (model is SxDbModel<TKey>)
                prepareUpdatedModel(model as SxDbModel<TKey>);
            _dbContext.Configuration.AutoDetectChangesEnabled = false;
            _dbContext.Entry(model).State = EntityState.Added;
            _dbContext.Configuration.AutoDetectChangesEnabled = true;
            _dbContext.SaveChanges();
            
            return model;
        }
        private static void prepareUpdatedModel(SxDbModel<TKey> model)
        {
            var date = DateTime.Now;
            if (model.DateCreate == DateTime.MinValue)
                model.DateCreate = date;
            if (model is SxDbUpdatedModel<TKey>)
            {
                var m = model as SxDbUpdatedModel<TKey>;
                if (m.DateUpdate == DateTime.MinValue)
                    m.DateUpdate = date;
                if (model is SxMaterial)
                    (model as SxMaterial).DateOfPublication = model.DateCreate;
            }
            
        }

        public virtual TModel Update(TModel model, bool changeDateUpdate=true, params string[] propertiesForChange)
        {
            var modelType = typeof(TModel);
            var keys = getEntityKeys(_dbContext, modelType, model);
            var oldModel = GetByKey(keys);
            if (oldModel == null) return null;
            var oldModelType = oldModel.GetType();
            var propsForChange = modelType.GetProperties()
                .Where(x => propertiesForChange.Contains(x.Name))
                .Select(x => new { Name = x.Name, Value = x.GetValue(model) }).ToArray();
            foreach (var prop in propsForChange)
            {
                var oldProp = oldModelType.GetProperty(prop.Name);
                oldProp.SetValue(oldModel, prop.Value);
            }

            if (changeDateUpdate && oldModel is SxDbUpdatedModel<TKey>)
                (oldModel as SxDbUpdatedModel<TKey>).DateUpdate = DateTime.Now;

            _dbContext.Configuration.AutoDetectChangesEnabled = false;
            _dbContext.Entry(oldModel).State = EntityState.Modified;
            _dbContext.Configuration.AutoDetectChangesEnabled = true;
            _dbContext.SaveChanges();

            return oldModel;
        }

        public virtual TModel Update(TModel model, object[] additionalData, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            return Update(model, changeDateUpdate, propertiesForChange);
        }

        public virtual void Delete(params object[] id)
        {
            var model = GetByKey(id);
            if (model == null) return;

            _dbContext.Entry(model).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        public virtual IQueryable<TModel> All
        {
            get
            {
                return _dbContext.Set<TModel>().AsNoTracking();
            }
        }

        public virtual IQueryable<TModel> Query(SxFilter filter)
        {
            return All;
        }

        public virtual int Count(SxFilter filter)
        {
            return All.Count();
        }

        public virtual TModel GetByKey(params object[] id)
        {
            var dbSet = _dbContext.Set<TModel>();
            return dbSet.Find(id);
        }

        private static object[] getEntityKeys(System.Data.Entity.DbContext dbContext, Type modelType, TModel model)
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var keyNames = ((IEnumerable<System.Data.Entity.Core.Metadata.Edm.EdmMember>)objectContext.MetadataWorkspace
                .GetType(modelType.Name, modelType.Namespace, System.Data.Entity.Core.Metadata.Edm.DataSpace.OSpace)
                .MetadataProperties
                .Where(x => x.Name == "KeyMembers")
                .First()
                .Value).Select(x => x.Name).ToArray();
            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = modelType.GetProperty(keyNames[i].ToString()).GetValue(model);
            }
            return keys;
        }
    }
}
