using Newtonsoft.Json;
using StructureHelperCommon.Models;

namespace DataAccess
{

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var logger = new TraceLogger();

//            // Create objects with complex relationships
//            var parent1 = new Parent { Name = "Parent_1" };
//            var parent2 = new Parent { Name = "Parent_2" };

//            var detail1 = new Detail { Description = "Detail_1", InternalNote = "Secret Note 1" };
//            var detail2 = new Detail { Description = "Detail_2", InternalNote = "Secret Note 2" };
//            var detail3 = new Detail { Description = "Detail_3", InternalNote = "Secret Note 3" };

//            var subDetail1 = new SubDetail { Info = "SubDetail_1" };

//            // Set up relationships
//            parent1.Details.Add(detail1);
//            parent1.Details.Add(detail2);

//            parent2.Details.Add(detail2); // Shared detail
//            parent2.Details.Add(detail3);

//            detail3.SubDetails.Add(subDetail1);

//            // Serialize with custom converters and trace logging
//            string json = Serialize(new List<Parent> { parent1, parent2 }, logger);
//            Console.WriteLine("Serialized JSON:");
//            Console.WriteLine(json);

//            // Deserialize with custom converters and trace logging
//            var deserializedParents = Deserialize<List<Parent>>(json, logger);

//            Console.WriteLine("\nDeserialized Objects:");
//            foreach (var parent in deserializedParents)
//            {
//                Console.WriteLine($"Parent: {parent.Name}, Id: {parent.Id}");
//                foreach (var detail in parent.Details)
//                {
//                    Console.WriteLine($"  Detail: {detail.Description}, Id: {detail.Id}");
//                }
//            }
//        }

//        static string Serialize(object obj, TraceLogger logger)
//        {
//            var settings = new JsonSerializerSettings
//            {
//                Converters = new List<JsonConverter>
//            {
//                new ParentConverter(logger),  // Add the specific converter
//                // Add other converters if needed
//            },
//                Formatting = Formatting.Indented
//            };

//            return JsonConvert.SerializeObject(obj, settings);
//        }

//        static T Deserialize<T>(string json, TraceLogger logger)
//        {
//            var settings = new JsonSerializerSettings
//            {
//                Converters = new List<JsonConverter>
//            {
//                new ParentConverter(logger),  // Add the specific converter
//                // Add other converters if needed
//            }
//            };

//            return JsonConvert.DeserializeObject<T>(json, settings);
//        }
//    }



//public class Parent
//    {
//        public Guid Id { get; set; } = Guid.NewGuid();

//        [JsonProperty("parent_name")]
//        public string Name { get; set; }

//        public List<Detail> Details { get; set; } = new List<Detail>();
//    }

//    public class Detail
//    {
//        public Guid Id { get; set; } = Guid.NewGuid();

//        [JsonPropertyName("detail_description")] // Compatible with System.Text.Json
//        public string Description { get; set; }

//        [JsonIgnore] // This property will be ignored during serialization
//        public string InternalNote { get; set; }

//        public List<SubDetail> SubDetails { get; set; } = new List<SubDetail>();
//    }

//    public class SubDetail
//    {
//        public Guid Id { get; set; } = Guid.NewGuid();
//        public string Info { get; set; }
//    }


}
