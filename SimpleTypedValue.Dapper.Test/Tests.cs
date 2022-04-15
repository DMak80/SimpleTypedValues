using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Xunit;

namespace SimpleTypedValue.Dapper.Test;

public class Tests
{
    public string SqlTables = @"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Entity2Dapper]') AND type in (N'U'))
DROP TABLE [dbo].[Entity2Dapper]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Entity1Dapper]') AND type in (N'U'))
DROP TABLE [dbo].[Entity1Dapper]

CREATE TABLE [dbo].[Entity1Dapper](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Entity1Dapper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Entity2Dapper](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Text2] [nvarchar](max) NOT NULL,
	[TestEntityId] [bigint] NOT NULL,
 CONSTRAINT [PK_Entity2Dapper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Entity2Dapper]  WITH CHECK ADD  CONSTRAINT [FK_Entity2Dapper_Entity1Dapper_TestEntityId] FOREIGN KEY([TestEntityId])
REFERENCES [dbo].[Entity1Dapper] ([Id])
ON DELETE CASCADE

insert into Entity1Dapper(Text) values('Entity 1'),('Entity 2');
insert into Entity2Dapper(Text2, TestEntityId) values('Entity2 1', 1),('Entity2 2', 2),('Entity2 3', 1);
";

    public Tests()
    {
        // typeof(TestEntityId).AddDapperTypedValueHandler();
        // typeof(TestEntity2Id).AddDapperTypedValueHandler();
    }

    private IDbConnection GetConnection()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString_Tests")
                               ?? "Server=(localdb)\\mssqllocaldb;Database=SimpleTypedValueDb;Trusted_Connection=True;";
        return new SqlConnection(connectionString);
    }

    [Fact]
    public void Test1()
    {
        using var db = GetConnection();
        db.Execute(SqlTables);
        var id = new TestEntityId(1);

        var sqlQuery = "select * from Entity1Dapper where Id=@id";
        var result = db.Query<TestEntity>(sqlQuery, new { id }).FirstOrDefault();
        Assert.Equal("Entity 1", result?.Text);
        Assert.Equal(id, result?.Id);
    }

    [Fact]
    public void Test2()
    {
        using var db = GetConnection();
        db.Execute(SqlTables);
        var id = new TestEntity2Id(1);
        var sqlQuery = @"
select e2.Id, Text, Text2 
from Entity1Dapper e1 
    inner join Entity2Dapper e2 
        on e1.Id=e2.TestEntityId 
where e2.Id=@id
";
        var res = db.Query<TestData>(sqlQuery, new { id }).FirstOrDefault();
        Assert.Equal("Entity 1", res?.Text);
        Assert.Equal("Entity2 1", res?.Text2);
        Assert.Equal(id, res?.Id);
    }

    public class TestData
    {
        public TestEntity2Id Id { get; set; }
        public string? Text { get; set; }
        public string? Text2 { get; set; }
    }
}