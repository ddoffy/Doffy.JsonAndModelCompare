// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Doffy.JsonAndModelCompare;
using Newtonsoft.Json.Linq;
using Refit;


// read json file as string
var json = File.ReadAllText("file.json");

// parse json string to JObject
var jObject = JObject.Parse(json);

// compare property name in Model class with property name in JObject recursively;
// if property name in Model class is different from property name in JObject use JsonProperty attribute to map property name in Model class to property name in JObject
var missedFields = GetMissedFields(jObject, typeof(Model), "Model");

if (missedFields.Count > 0)
{
    Console.WriteLine("Missed fields:");
    foreach (var missedField in missedFields)
    {
        Console.WriteLine(missedField);
    }
}

var authApi = RestService.For<IAuthApi>(new HttpClient(new CustomeHttpMessageHandler(new HttpClientHandler()))
{
    BaseAddress = new Uri("https://localhost:5001"),
});

var respoinse = await authApi.Login(new LoginRequest()
{
    UserName = "username",
});


static List<string> GetMissedFields(JObject jObject, Type type, string prefix = "")
{
    var missedFields = new List<string>();
    var properties = jObject.Properties();
    var modelProperties = type.GetProperties().Select(x => x.Name);

    foreach (var property in properties)
    {
        var modelPropertyName = modelProperties.FirstOrDefault(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
        if (modelPropertyName == null)
        {
            var fullName = $"{prefix}.{property.Name}";
            // Console.WriteLine($"Property {fullName} not found in Model class");
            missedFields.Add(fullName);
        }
        // modelProperty.PropertyType is a class
        else
        {
            var modelProperty = type.GetProperty(modelPropertyName);
            if (modelProperty.PropertyType.IsClass && modelProperty.PropertyType != typeof(string))
            {
                var subJObject = property.Value as JObject;
                if (subJObject == null)
                {
                    var fullName = $"{prefix}.{property.Name}";
                    // Console.WriteLine($"Property {fullName} is not JObject");
                    missedFields.Add(fullName);
                    continue;
                }

                missedFields.AddRange(GetMissedFields(subJObject, modelProperty.PropertyType, $"{prefix}.{property.Name}"));
            }
        }
    }

    return missedFields;
}

//
// var model = new Model
// {
//     Name = "Name",
//     Age = 10,
//     SubModel = new SubModel
//     {
//         Name = "SubName",
//         Age = 20
//     },
//     SubArrayModel = new SubArrayModel
//     {
//         Name = "SubArrayName",
//         Age = 30,
//         SubModels = new SubModel[]
//         {
//             new SubModel
//             {
//                 Name = "SubArrayName1",
//                 Age = 40
//             },
//             new SubModel
//             {
//                 Name = "SubArrayName2",
//                 Age = 50
//             }
//         }
//     }
// };
//
// var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
// {
//     WriteIndented = true
// });
//
// Console.WriteLine(json);
//
// File.WriteAllText("file.json", json);