using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpSample
{
    public class DataModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("recurrenceRule")]
        public object RecurrenceRule { get; set; }
        
        [JsonProperty("Id")]
        public int Id { get; set; }
        
        [JsonProperty("recurrenceException")]
        public object RecurrenceException { get; set; }
        
        [JsonProperty("allDay")]
        public bool AllDay { get; set; }
        
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
       
        [JsonProperty("ResourceTypeIds")]
        public Dictionary<string, int[]> ResourceTypeIds { get; set; }
    }

    //2168866.aspx?Parsing+JSON+data+Issue
    //Parsing JSON data Issue
    public class ParseJson2168866
    {
        public static DataModel ParseData()
        {
            string json = "{\"text\":\"TestallDay\",\"description\":null,\"recurrenceRule\":null,\"id\":238,\"recurrenceException\":null,\"allDay\":true,\"startDate\":\"2020-07-07T05:00:00Z\",\"endDate\":\"2020-07-08T05:00:00Z\",\"resourceTypeId_52\":[134],\"resourceTypeId_49\":[118,124]}";
            
            var data = JsonConvert.DeserializeObject<DataModel>(json);
            data.ResourceTypeIds = new Dictionary<string, int[]>();
            JObject item = JObject.Parse(json);

            /*
            IEnumerable<JProperty> props = item.Properties().Where(p => p.Name.Contains("resourceTypeId"));
            foreach (var prop in props)
            {
                List<int> arrayItems = new List<int>();
                foreach (var arrItem in prop.Value)
                {
                    arrayItems.Add((int)arrItem);
                }
                data.ResourceTypeIds.Add(prop.Name, arrayItems.ToArray());
            }
            */
            IEnumerable<JProperty> props = item.Properties().Where(p => p.Name.Contains("resourceTypeId"));
            foreach (var (prop, arrayItems) in from prop in props
                                               let arrayItems = (from arrItem in prop.Value
                                                                 select (int)arrItem).ToList()
                                               select (prop, arrayItems))
            {
                data.ResourceTypeIds.Add(prop.Name, arrayItems.ToArray());
            }

            return data;
        }
    }
}



