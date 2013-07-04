using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add the following using statements:
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MinimalWebApiClientDemo
{
    class ClassBasedExamples
    {
        public static void PrintClassExamples()
        {
            string personOutputString = "id: {0};  LastName: {1}, FirstName: {2}";

            // WRITE ALL PEOPLE TO CONSOLE
            Console.WriteLine("Retreive All The People using classes:");
            IEnumerable<Person> people = ClassBasedExamples.getAllPeople();
            foreach (var person in people)
            {
                Console.WriteLine(personOutputString, person.Id, person.LastName, person.FirstName);
            }

            // WRITE A SPECIFIC PERSON TO CONSOLE:
            Console.WriteLine(Environment.NewLine 
                + "Retreive a Person object by ID:");

            Person singlePerson = ClassBasedExamples.getPerson(2);
            Console.WriteLine(personOutputString, singlePerson.Id, 
                singlePerson.LastName, singlePerson.FirstName);

            // ADD NEW PERSON, THEN WRITE TO CONSOLE:
            Console.WriteLine(Environment.NewLine 
                + "Add a new Person object and return the new object:");

            Person newPerson = new Person { LastName = "Atten", FirstName = "John" };
            newPerson = AddPerson(newPerson);
            Console.WriteLine(personOutputString, newPerson.Id, 
                newPerson.LastName, newPerson.FirstName);

            // UPDATE AN EXISTING PERSON, THEN WRITE TO CONSOLE:
            Console.WriteLine(Environment.NewLine 
                + "Update an existing Person object:");

            // Pretend we already had a person's data:
            Person personToUpdate = getPerson(2);
            string newLastName = "Richards";

            Console.WriteLine("Updating Last Name of " 
                + personToUpdate.LastName + " to " + newLastName);
            personToUpdate.LastName = newLastName;

            if (ClassBasedExamples.UpdatePerson(personToUpdate))
            {
                Console.WriteLine(Environment.NewLine + "Updated person object:");
                Person updatedPerson = getPerson(2);
                Console.WriteLine(personOutputString, updatedPerson.Id, 
                    updatedPerson.LastName, updatedPerson.FirstName);
            }


            // DELETE AN EXISTING PERSON BY ID:
            Console.WriteLine(Environment.NewLine + "Delete person object:");

            ClassBasedExamples.DeletePerson(5);

            // WRITE THE UPDATED LIST TO THE CONSOLE:
            {
                Console.WriteLine("Retreive All The People using classes:");
                people = ClassBasedExamples.getAllPeople();
                foreach (var person in people)
                {
                    Console.WriteLine(personOutputString, person.Id, 
                        person.LastName, person.FirstName);
                }
            }

            Console.Read();
        }


        // DEFINE A PERSON CLASS IDENTICAL TO THE ONE IN THE API:
        public class Person
        {
            public int Id { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
        }


        static IEnumerable<Person> getAllPeople()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = 
                client.GetAsync("http://localhost:57772/api/person/").Result;
            client.Dispose();

            return response.Content.ReadAsAsync<IEnumerable<Person>>().Result;
        }


        static Person getPerson(int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = 
                client.GetAsync("http://localhost:57772/api/person/" + id).Result;
            client.Dispose();
            return response.Content.ReadAsAsync<Person>().Result;
        }


        static Person AddPerson(Person newPerson)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57772/");
            var response = client.PostAsJsonAsync("api/person", newPerson).Result;
            client.Dispose();
            return response.Content.ReadAsAsync<Person>().Result;
        }


        static bool UpdatePerson(Person newPerson)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57772/");
            var response = client.PutAsJsonAsync("api/person/", newPerson).Result;
            client.Dispose();
            return response.Content.ReadAsAsync<bool>().Result;
        }


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
