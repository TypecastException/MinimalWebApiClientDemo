// Add the following using statements:
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MinimalWebApiClientDemo
{
    class JsonExamples
    {
        public static void PrintJsonExamples()
        {
            // WRITE ALL PEOPLE TO CONSOLE (JSON):
            Console.WriteLine("Retreive All The People:");
            JArray people = getAllPeople();
            foreach (var person in people)
            {
                Console.WriteLine(person);
            }

            // WRITE A SPECIFIC PERSON TO CONSOLE (JSON):
            Console.WriteLine(Environment.NewLine + "Retreive a Person by ID:");
            JObject singlePerson = getPerson(2);
            Console.WriteLine(singlePerson);

            // ADD NEW PERSON, THEN WRITE TO CONSOLE (JSON):
            Console.WriteLine(Environment.NewLine + "Add a new Person and return the new object:");
            JObject newPerson = AddPerson("Atten", "John");
            Console.WriteLine(newPerson);

            // UPDATE AN EXISTING PERSON, THEN WRITE TO CONSOLE (JSON):
            Console.WriteLine(Environment.NewLine + "Update an existing Person and return a boolean:");

            // Pretend we already had a person's data:
            JObject personToUpdate = getPerson(2);
            string newLastName = "Richards";

            Console.WriteLine("Update Last Name of " + personToUpdate + "to " + newLastName);

            // Pretend we don't already know the Id:
            int id = personToUpdate.Value<int>("Id");
            string FirstName = personToUpdate.Value<string>("FirstName");
            string LastName = personToUpdate.Value<string>("LastName");

            if (UpdatePerson(id, newLastName, FirstName))
            {
                Console.WriteLine(Environment.NewLine + "Updated person:");
                Console.WriteLine(getPerson(id));
            }

            // DELETE AN EXISTING PERSON BY ID:
            Console.WriteLine(Environment.NewLine + "Delete person object:");
            JsonExamples.DeletePerson(5);

            // WRITE THE UPDATED LIST TO THE CONSOLE:
            {
                // WRITE ALL PEOPLE TO CONSOLE
                Console.WriteLine("Retreive All The People using classes:");
                people = JsonExamples.getAllPeople();
                foreach (var person in people)
                {
                    Console.WriteLine(person);
                }
            }

            Console.Read();
        }


        // Sends HTTP GET to Person Controller on API:
        static JArray getAllPeople()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = 
                client.GetAsync("http://localhost:57772/api/person/").Result;
            return response.Content.ReadAsAsync<JArray>().Result;
        }


        // Sends HTTP GET to Person Controller on API with ID:
        static JObject getPerson(int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = 
                client.GetAsync("http://localhost:57772/api/person/" + id).Result;
            return response.Content.ReadAsAsync<JObject>().Result;
        }


        // Sends HTTP POST to Person Controller on API with Anonymous Object:
        static JObject AddPerson(string newLastName, string newFirstName)
        {
            // Initialize an anonymous object representing a new Person record:
            var newPerson = new { LastName = newLastName, FirstName = newFirstName };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57772/");
            var response = client.PostAsJsonAsync("api/person", newPerson).Result;
            return response.Content.ReadAsAsync<JObject>().Result;
        }


        // Sends HTTP PUT to Person Controller on API with Anonymous Object:
        static bool UpdatePerson(int personId, string newLastName, string newFirstName)
        {
            // Initialize an anonymous object representing a the modified Person record:
            var newPerson = new { id = personId, LastName = newLastName, FirstName = newFirstName };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57772/");
            var response = client.PutAsJsonAsync("api/person/", newPerson).Result;
            return response.Content.ReadAsAsync<bool>().Result;
        }


        // Sends HTTP DELETE to Person Controller on API with Id Parameter:
        static void DeletePerson(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57772/");
            var relativeUri = "api/person/" + id.ToString();
            var response = client.DeleteAsync(relativeUri).Result;
            client.Dispose();
        }
    }
}
