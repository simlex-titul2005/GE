using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SX.WebCore.Abstract
{
    public abstract class SxDbRepository<TKey, TModel, TDbContext>
        where TModel : SxDbModel<TKey>
        where TDbContext : SxDbContext
    {
        private SxDbContext _dbContext;
        public SxDbRepository()
        {
            _dbContext = Activator.CreateInstance<TDbContext>();
        }

        public virtual TModel Create(TModel model)
        {
            _dbContext.Entry(model).State = EntityState.Added;
            _dbContext.SaveChanges();
            return model;
        }

        public virtual TModel Update(TModel model, params string[] propertiesForChange)
        {
            var modelType = typeof(TModel);
            var keys = getEntityKeys(_dbContext, modelType, model);
            var oldModel = GetByKey(keys);
            if (oldModel == null) return null;
            var oldModelType = oldModel.GetType();
            var propsForChange = modelType.GetProperties()
                .Where(x => propertiesForChange.Contains(x.Name))
                .Select(x => new { Name = x.Name, Value = x.GetValue(model) });
            foreach (var prop in propsForChange)
            {
                var oldProp = oldModelType.GetProperty(prop.Name);
                oldProp.SetValue(oldModel, prop.Value);
            }

            if (oldModel is SxDbUpdatedModel<TKey>)
                (oldModel as SxDbUpdatedModel<TKey>).DateUpdate = DateTime.Now;

            _dbContext.Entry(oldModel).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return oldModel;
        }

        public virtual void Delete(params object[] id)
        {
            var model = GetByKey(id);
            if (model == null) return;

            _dbContext.Entry(model).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        public virtual IEnumerable<TModel> All
        {
            get
            {
                return _dbContext.Set<TModel>();
            }
        }

        public virtual TModel GetByKey(params object[] id)
        {
            var dbSet = _dbContext.Set<TModel>();
            return dbSet.Find(id);
        }

        private static object[] getEntityKeys(System.Data.Entity.DbContext dbContext, Type modelType, TModel model)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var keyNames = ((IEnumerable<System.Data.Entity.Core.Metadata.Edm.EdmMember>)objectContext.MetadataWorkspace
                .GetType(modelType.Name, modelType.Namespace, System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace)
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
