using Microsoft.EntityFrameworkCore;

namespace SimpleTypedValue.EF.Test;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestOldEntity> OldEntities => Set<TestOldEntity>();
    public DbSet<TestEntity> Entity1 => Set<TestEntity>();
    public DbSet<TestEntity2> Entity2 => Set<TestEntity2>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TestOldEntity>()
            .Property(x => x.Id)
            //.ValueGeneratedOnAdd()
            ;

        modelBuilder.Entity<TestEntity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            ;

        modelBuilder.Entity<TestEntity2>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            ;

        modelBuilder.UseTypedValues(Database.IsInMemory());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTypedValues();
    }

    public void Init()
    {
        var oldEntity = new TestOldEntity
        {
            //Id = 1,
            Text = "Old Entity"
        };
        var entity_1 = new TestEntity
        {
            //Id = 1L.AsTypedValue<TestEntityId, long>(),
            Text = "Entity 1"
        };
        var entity_2 = new TestEntity
        {
            //Id = 2L.AsTypedValue<TestEntityId, long>(),
            Text = "Entity 2"
        };
        var entity2_1 = new TestEntity2
        {
            //Id = 1L.AsTypedValue<TestEntity2Id, long>(),
            Text2 = "Entity2 1",
            TestEntity = entity_1
        };
        var entity2_2 = new TestEntity2
        {
            //Id = 2L.AsTypedValue<TestEntity2Id, long>(),
            Text2 = "Entity2 2",
            TestEntity = entity_2
        };
        var entity2_3 = new TestEntity2
        {
            //Id = 3L.AsTypedValue<TestEntity2Id, long>(),
            Text2 = "Entity2 3",
            TestEntity = entity_1
        };

        //Add(oldEntity);
        AddRange(entity_1, entity_2, entity2_1, entity2_2, entity2_3);
        SaveChanges();
    }
}