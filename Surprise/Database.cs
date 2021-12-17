using Android.Content.PM;
using AndroidX.Core.App;

using Neo4jClient;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surprise
{
    public partial class MainActivity
    {
        public class Database
        {
            // [ Free Cloud Database]

            // Aura queries use an encrypted connection using the "neo4j+s" protocol
            string uri = "neo4j+s://cfae4ea5.databases.neo4j.io";

            string user = "neo4j";
            string pass = "jo4PxjbDvU7nMLZSPPYTnOcXusihLdgWsbYE_khdxeo";

            GraphClient client;

            // Connect to Database
            public int Connect()
            {
                try
                {
                    client = new GraphClient(new Uri(uri), user, pass);
                    client.ConnectAsync();

                    return 0;
                }
                catch
                {
                    return 1;
                }
            }

            // Upload to Database
            public int Upload(string name, string number)
            {
                try
                {
                    client.Cypher.Create("x:bot { name: $name, number: $number }");
                    return 0;
                }
                catch
                {
                    return 1;
                }
            }
        }

        public class DriverIntroductionExample : IDisposable
        {
            private bool _disposed = false;
            private readonly IDriver _driver;

            ~DriverIntroductionExample() => Dispose(false);

            public DriverIntroductionExample(string uri, string user, string password)
            {
                _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            }

            public async Task CreateFriendship(string person1Name, string person2Name)
            {
                // To learn more about the Cypher syntax, see https://neo4j.com/docs/cypher-manual/current/
                // The Reference Card is also a good resource for keywords https://neo4j.com/docs/cypher-refcard/current/
                var query = @"
        MERGE (p1:Person { name: $person1Name })
        MERGE (p2:Person { name: $person2Name })
        MERGE (p1)-[:KNOWS]->(p2)
        RETURN p1, p2";

                var session = _driver.AsyncSession();
                try
                {
                    // Write transactions allow the driver to handle retries and transient error
                    var writeResults = await session.WriteTransactionAsync(async tx =>
                    {
                        var result = await tx.RunAsync(query, new { person1Name, person2Name });
                        return (await result.ToListAsync());
                    });

                    foreach (var result in writeResults)
                    {
                        var person1 = result["p1"].As<INode>().Properties["name"];
                        var person2 = result["p2"].As<INode>().Properties["name"];
                        Console.WriteLine($"Created friendship between: {person1}, {person2}");
                    }
                }
                // Capture any errors along with the query and data for traceability
                catch (Neo4jException ex)
                {
                    Console.WriteLine($"{query} - {ex}");
                    throw;
                }
                finally
                {
                    await session.CloseAsync();
                }
            }

            public async Task FindPerson(string personName)
            {
                var query = @"
        MATCH (p:Person)
        WHERE p.name = $name
        RETURN p.name";

                var session = _driver.AsyncSession();
                try
                {
                    var readResults = await session.ReadTransactionAsync(async tx =>
                    {
                        var result = await tx.RunAsync(query, new { name = personName });
                        return (await result.ToListAsync());
                    });

                    foreach (var result in readResults)
                    {
                        Console.WriteLine($"Found person: {result["p.name"].As<String>()}");
                    }
                }
                // Capture any errors along with the query and data for traceability
                catch (Neo4jException ex)
                {
                    Console.WriteLine($"{query} - {ex}");
                    throw;
                }
                finally
                {
                    await session.CloseAsync();
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                if (disposing)
                {
                    _driver?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
