using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SimpleTypedValue.EF.Test;

public class Tests
{
    [Fact]
    public void Test1()
    {
        var db = DbInMemory(true);
        db.Init();
        var id = new TestEntityId(1L);
        var res = db.Entity1
            .Where(x => x.Id == id)
            .Select(x => x.Text)
            .FirstOrDefault();
        Assert.Equal("Entity 1", res);
    }
    
    [Fact]
    public void Test2()
    {
        var db = DbInMemory(true);
        db.Init();
        var id = new TestEntity2.ID(1L);
        var res = db.Entity2
            .Where(x => x.Id == id)
            .Select(x => new { x.TestEntity!.Text, x.Text2 })
            .FirstOrDefault();
        Assert.Same("Entity 1", res?.Text);
        Assert.Same("Entity2 1", res?.Text2);
    }

    [Fact]
    public void Test3()
    {
        var db = DbSqlServer(true);
        db.Init();
        var id = new TestEntityId(1);
        var res = db.Entity1
            .Where(x => x.Id == id)
            .Select(x => x.Text)
            .FirstOrDefault();
        Assert.Equal("Entity 1", res);
    }
    
    [Fact]
    public void Test4()
    {
        var db = DbSqlServer(true);
        db.Init();
        var id = new TestEntity2.ID(1);
        var res = db.Entity2
            .Where(x => x.Id == id)
            .Select(x => new { x.TestEntity!.Text, x.Text2 })
            .FirstOrDefault();
        Assert.Equal("Entity 1", res?.Text);
        Assert.Equal("Entity2 1", res?.Text2);
    }
    
    private TestDbContext DbInMemory(bool recreate = false)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>();
        options.UseInMemoryDatabase("TestDb");

        if (recreate)
            DbContextExtension.ResetInMemorySimpleTypedIntegerValueGeneratorFactory();

        return Db(options.Options, recreate);
    }

    private TestDbContext DbSqlServer(bool recreate = false)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString_Tests")
                               ?? "Server=(localdb)\\mssqllocaldb;Database=SimpleTypedValueDb;Trusted_Connection=True;";
        options.UseSqlServer(connectionString);

        return Db(options.Options, recreate);
    }

    private TestDbContext Db(DbContextOptions<TestDbContext> options, bool recreate = false)
    {
        using (var db = new TestDbContext(options))
        {
            if (recreate)
                db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        return new TestDbContext(options);
    }
}