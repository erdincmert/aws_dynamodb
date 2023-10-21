using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure AWS credentials
        var credentials = new BasicAWSCredentials("AccessKey", "SecretKey");
        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.EUCentral1 // Change this to your desired region
        };

        // Create DynamoDB client
        var client = new AmazonDynamoDBClient(credentials, config);

        // Define the table schema
        var tableName = "Test";
        var keySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement
            {
                AttributeName = "PrimaryKey",
                KeyType = KeyType.HASH
            }
        };

        var attributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition
            {
                AttributeName = "PrimaryKey",
                AttributeType = ScalarAttributeType.S
            }
        };

        // Create the table
        try
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                KeySchema = keySchema,
                AttributeDefinitions = attributeDefinitions
            };

            var response = await client.CreateTableAsync(request);

            Console.WriteLine("Table creation status: " + response.TableDescription.TableStatus);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error creating table: " + ex.Message);
        }
        Console.WriteLine("done");

        // Insert an item
        try
        {
            var putRequest = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "PrimaryKey", new AttributeValue { S = "YourPrimaryKeyValue" } },
                    { "AttributeName1", new AttributeValue { S = "Value1" } },
                    { "AttributeName2", new AttributeValue { N = "42" } }
                }
            };

            var putResponse = await client.PutItemAsync(putRequest);

            Console.WriteLine("Item inserted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting item: " + ex.Message);
        }
    }
}
