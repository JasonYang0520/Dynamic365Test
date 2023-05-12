using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace JasonDemoConsole
{
    class Program
    {
        private static IOrganizationService service;

        static void Main(string[] args)
        {
            try
            {
                Load();
                //CreateRecord();
                //UpdateRecord();
                //DeleteRecord();
                //FetchRecord();
                //FetchAllRecord();
                //QueryRecordDemo();
                QueryRecordDemoLink();
                WhoAmIRequest test = new WhoAmIRequest();
                var temp = service.Execute(test);

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void Load()
        {
            Uri organizationUri = new Uri("http://s16c/CRMLab01/XRMServices/2011/Organization.svc");
            ClientCredentials cred = new ClientCredentials();
            cred.UserName.UserName = "User07";
            cred.UserName.Password = "Pa$$w0rd";
            var serviceproxy = new OrganizationServiceProxy(organizationUri, null, cred, null);
            service = serviceproxy;
        }

        private static void CreateRecord()
        {
            Entity demo = new Entity("jason_demo");

            demo["jason_name"] = "C#";
            demo["jason_text"] = "aaa";
            demo["jason_optionset"] = new OptionSetValue(0);
            demo["jason_multioptionset"] = new OptionSetValueCollection
            {
                new OptionSetValue(1),
                new OptionSetValue(2)
            };
            demo["jason_twooption"] = true;
            demo["jason_int"] = 123;
            demo["jason_float"] = 123.11;
            demo["jason_decimal"] = (decimal)123.45;
            demo["jason_currency"] = new Money(new decimal(123.12));
            demo["jason_multitext"] = "123\n123";
            demo["jason_datetime"] = DateTime.Now;
            demo["jason_accountid"] = new EntityReference("account", new Guid("498a6867-a5ef-ed11-befe-e0d55e847993"));

            service.Create(demo);
        }

        private static void UpdateRecord()
        {
            Entity demo = new Entity("jason_demo")
            {
                Id = new Guid("6158bca2-72f0-ed11-beff-e0d55e847966")
            };
            demo["jason_text"] = "updateeeeee";
            service.Update(demo);
        }
        private static void DeleteRecord()
        {
            service.Delete("jason_demo", new Guid("6158bca2-72f0-ed11-beff-e0d55e847966"));
        }

        private static void FetchRecord()
        {
            var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='jason_demo'>
                                <attribute name='jason_demoid' />
                                <attribute name='jason_name' />
                                <attribute name='createdon' />
                                <order attribute='jason_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                  <condition attribute='jason_name' operator='eq' value='a' />
                                </filter>
                              </entity>
                            </fetch>";

            var ec = service.RetrieveMultiple(new FetchExpression(fetchXml));

            foreach (var item in ec.Entities)
            {
                Console.WriteLine(item["jason_name"].ToString());
            }
        }


        private static void FetchAllRecord()
        {
            var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='jason_demo'>
                                    <all-attributes />
                                  </entity>
                                </fetch>";

            var ec = service.RetrieveMultiple(new FetchExpression(fetchXml));

            foreach (var item in ec.Entities)
            {
                Console.WriteLine(item["jason_name"].ToString());
                Console.WriteLine(item["jason_text"].ToString());
                Console.WriteLine((OptionSetValue)item["jason_optionset"]);
                Console.WriteLine((OptionSetValueCollection)item["jason_multioptionset"]);
                Console.WriteLine((Boolean)item["jason_twooption"]);
                Console.WriteLine((int)item["jason_int"]);
                Console.WriteLine((double)item["jason_float"]);
                Console.WriteLine((Decimal)item["jason_decimal"]);
                Console.WriteLine((Money)item["jason_currency"]);
                Console.WriteLine(item["jason_multitext"].ToString());
                Console.WriteLine((DateTime)item["jason_datetime"]);
                Console.WriteLine((EntityReference)item["jason_accountid"]);
                Console.WriteLine("============");
            }
        }

        private static void QueryRecordDemo()
        {
            var query = new QueryExpression("jason_demo")
            {
                //ColumnSet = new ColumnSet(new string[] { "jason_name", "jason_text" })                
                ColumnSet = new ColumnSet(true)                // 全查
            };
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition("jason_demoid", ConditionOperator.Equal, "13e1953e-caef-ed11-befe-e0d55e847993");

            var ec = service.RetrieveMultiple(query);

            foreach (var item in ec.Entities)
            {
                Console.WriteLine(item["jason_name"].ToString());
            }
        }

        private static void QueryRecordDemoLink()
        {
            var query = new QueryExpression("jason_demo")
            {
                ColumnSet = new ColumnSet(true)
            };
            LinkEntity linkEntity = new LinkEntity("jason_demo", "jason_jasondemochild", "jason_demoid", "jason_demoid", JoinOperator.Inner);
            linkEntity.LinkCriteria = new FilterExpression(LogicalOperator.And);
            linkEntity.LinkCriteria.AddCondition("jason_name", ConditionOperator.Equal, "000");

            query.LinkEntities.Add(linkEntity);

            var ec = service.RetrieveMultiple(query);

            foreach (var item in ec.Entities)
            {
                Console.WriteLine(item["jason_name"].ToString());
            }
        }

    }
}
