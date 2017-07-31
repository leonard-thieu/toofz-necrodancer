using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlBulkUpsert.Tests
{
    [TestClass]
    public class TypedUpserterTests : DatabaseTestsBase
    {
        [TestMethod]
        public async Task EndToEnd()
        {
            using (var connection = DatabaseHelper.CreateAndOpenConnection())
            {
                // Arrange
                var targetSchema = await SqlTableSchema.LoadFromDatabaseAsync(connection, "TestUpsert", CancellationToken.None);

                var columnMappings = new ColumnMappings<TestDto>
                {
                    { "key_part_1", d => d.KeyPart1 },
                    { "key_part_2", d => d.KeyPart2 },
                    { "nullable_text", d => d.Text },
                    { "nullable_number", d => d.Number },
                    { "nullable_datetimeoffset", d => d.Date },
                };

                var upserter = new TypedUpserter<TestDto>(targetSchema, columnMappings);

                var items = new List<TestDto>();

                for (int i = 1; i <= 10; i++)
                {
                    items.Add(new TestDto
                    {
                        KeyPart1 = "TEST",
                        KeyPart2 = (short)i,
                        Text = String.Format("some text here {0}", i),
                        Number = i,
                        Date = new DateTimeOffset(new DateTime(2010, 11, 14, 12, 0, 0), TimeSpan.FromHours(i)),
                    });
                }

                // Act
                await upserter.UpsertAsync(connection, items, null, false, CancellationToken.None);

                // Assert
                foreach (var testDto in items)
                {
                    Assert.AreEqual(testDto.Number, testDto.KeyPart2);
                }
            }
        }

        class TestDto
        {
            public string KeyPart1 { get; set; }
            public short KeyPart2 { get; set; }
            public string Text { get; set; }
            public int Number { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}