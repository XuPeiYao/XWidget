using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using XWidget.EFLogic.Test.Models;

namespace XWidget.EFLogic.Test {
    public class RemoveExtensionsTest {
        [Fact]
        public void RemoveCascadeTypeTest() {
            var context = TestContext.CreateInstance();

            Assert.Empty(
                new Type[] { typeof(Category), typeof(Note), typeof(UserData) }.Except(
                    context.GetRemoveCascadeTypes(typeof(Category))
                ));

            Assert.Empty(
                new Type[] { typeof(Note), typeof(UserData) }.Except(
                    context.GetRemoveCascadeTypes(typeof(Note))
                ));

            Assert.Empty(
                new Type[] { typeof(UserData) }.Except(
                    context.GetRemoveCascadeTypes(typeof(UserData))
                ));
        }

        [Fact]
        public void RemoveCascadeTest() {
            var context = TestContext.CreateInstance();

            var category_A = context.Categories.SingleOrDefault(x => x.Name == "Category_A");
            var category_B = context.Categories.SingleOrDefault(x => x.Name == "Category_B");

            Assert.NotNull(category_A);
            Assert.NotNull(category_B);

            var category_A_notes = context.Notes.Where(x => x.Category == category_A).ToArray();

            Assert.NotEmpty(context.Notes.Where(x => x.Category == category_A));
            Assert.NotEmpty(context.Categories.Where(x => x.ParentId == category_A.Id));

            context.RemoveCascade(category_A);

            context.SaveChanges();

            Assert.Empty(context.Notes.Where(x => x.Category == category_A));
            Assert.Empty(context.Categories.Where(x => x.ParentId == category_A.Id));

            Assert.NotEmpty(context.Notes.Where(x => x.Category == category_B));
            Assert.NotEmpty(context.Categories.Where(x => x.ParentId == category_B.Id));

            var category_B_FirstChild = category_B.Children.FirstOrDefault();

            Assert.NotNull(category_B_FirstChild);
            Assert.NotEmpty(context.Notes.Where(x => x.Category == category_B_FirstChild));

            context.RemoveCascade(category_B_FirstChild);
            context.SaveChanges();

            Assert.NotEmpty(context.Notes.Where(x => x.Category == category_B));
            Assert.NotEmpty(context.Categories.Where(x => x.ParentId == category_B.Id));


            Assert.NotEmpty(context.Notes.Where(x => x.Title == "Note_8"));
            Assert.NotEmpty(context.UserDatas.Where(x => x.Name == "User2"));
            context.RemoveCascade(context.UserDatas.First(x => x.Name == "User2"));
            context.SaveChanges();
            Assert.NotEmpty(context.Notes.Where(x => x.Title == "Note_8"));
            Assert.Empty(context.UserDatas.Where(x => x.Name == "User2"));


            Assert.NotEmpty(context.Notes.Where(x => x.Title == "Note_7"));
            context.RemoveCascade(category_B);
            context.SaveChanges();
            Assert.Empty(context.Notes.Where(x => x.Title == "Note_7"));
        }
    }
}
