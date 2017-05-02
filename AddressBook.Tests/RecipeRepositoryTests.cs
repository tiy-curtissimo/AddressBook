using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace AddressBook.Tests
{
    [TestFixture]
    public class RecipeRepositoryTests
    {
        public RecipeRepositoryTests()
        {
            _connectionString = "Server=localhost;Database=AddressBook_Test;Trusted_Connection=True";
        }

        [SetUp]
        public void CreateDatabaseAndTable()
        {
            try
            {
                using (var cnx = new SqlConnection("Server=localhost;Database=master;Trusted_Connection=True"))
                {
                    cnx.Open();
                    var cmd = cnx.CreateCommand();
                    cmd.CommandText = "CREATE DATABASE AddressBook_Test";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { }
            using (var cnx = new SqlConnection(_connectionString))
            {
                cnx.Open();
                var cmd = cnx.CreateCommand();
                cmd.CommandText = "CREATE TABLE Recipes (RecipeId INT PRIMARY KEY IDENTITY(1, 1) NOT NULL, RecipeTypeId INT NOT NULL, Name NVARCHAR(200) NOT NULL)";
                cmd.ExecuteNonQuery();
            }
        }

        [Test, Category("Integration")]
        public void CreateRecipePutsSomethingInTheDatabase()
        {
            var repo = new RecipesRepository(_connectionString);
            repo.Create("Pineapple on a stick", RecipeType.Desserts);
            var results = repo.GetAllRecipes();

            Assert.That(results.Count, Is.EqualTo(1));
        }

        [TearDown]
        public void DropDatabase()
        {
            using (var cnx = new SqlConnection(_connectionString))
            {
                cnx.Open();
                var cmd = cnx.CreateCommand();
                cmd.CommandText = "DROP TABLE Recipes";
                cmd.ExecuteNonQuery();
            }
        }

        private readonly string _connectionString;
    }
}
