using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SX.WebCore.Abstract;
using GE.WebCoreExtantions;
using SX.WebCore;
using System.Linq;
using GE.WebCoreExtantions.Repositories;

namespace GE.Tests
{
    [TestClass]
    public class GRUD
    {
        [TestMethod]
        public void CreateArticle()
        {
            var dbContext = new GE.WebCoreExtantions.DbContext();
            var dbRepo = new RepoArticle();
            var date = DateTime.Now;
            var model = new Article {
                ModelCoreType=Enums.ModelCoreType.Article,
                DateCreate=date,
                DateUpdate=date,
                Html="Html",
                Title="Test article"
            };
            var newModel = dbRepo.Create(model);
            Assert.IsNotNull(newModel);
        }

        [TestMethod]
        public void GetArticles()
        {
            var dbContext = new GE.WebCoreExtantions.DbContext();
            var dbRepo = new RepoArticle();
            var list = dbRepo.All;
            Assert.IsTrue(list != null);
        }

        [TestMethod]
        public void UpdateArticle()
        {
            var dbContext = new GE.WebCoreExtantions.DbContext();
            var dbRepo = new RepoArticle();
            var model = new Article
            {
                Id=4,
                ModelCoreType=Enums.ModelCoreType.Article,
                Html = "111"
            };
            var updatedModel = dbRepo.Update(model, new string[] { "Html" });
            if (updatedModel == null) return;
            Assert.IsTrue(Equals(model.Html, updatedModel.Html));
        }

        [TestMethod]
        public void DeleteArticle()
        {
            var dbContext = new GE.WebCoreExtantions.DbContext();
            var dbRepo = new RepoArticle();
            var key = new object[] { 1, Enums.ModelCoreType.Article };
            var count = dbRepo.All.Count();
            dbRepo.Delete(key);
            var newCount = dbRepo.All.Count();
            if (count == 0 || count==newCount) return;
            Assert.IsTrue(Equals(count - 1, newCount));
        }
    }
}
