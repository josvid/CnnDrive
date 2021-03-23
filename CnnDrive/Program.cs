using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CnnDrive
{
    class Program
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static void Main(string[] args)
        {
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open,FileAccess.Read))
            {
                string creadPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
                Console.WriteLine("Se ha creado el token");
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            string spreadsheetId = "xxxxxxx";
            string range = "Inventario!A:C";

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId,range);
            var response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if(values!=null && values.Count>0)
            {
                foreach(var row in values)
                {
                    //Console.WriteLine(row[0]);
                    Console.WriteLine("{0}, {1}, {2}", row[0], row[1], row[2]);
                }
            }
            else
            {
                Console.WriteLine("No hay ni madres");
            }
            Console.Read();
        }
    }
}
