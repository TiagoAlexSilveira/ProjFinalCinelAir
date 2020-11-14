using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Collections.Generic;
using System.Linq;


namespace ProjFinalCinelAirClient.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        public Client GetClientByUserId(string id)
        {
            var client = _context.Client.FirstOrDefault(u => u.UserId == id);

            return client;
        }


        public Client GetClientByUserEmail(string email)
        {
            var client = _context.Client.FirstOrDefault(e => e.User.Email == email);

            return client;
        }

        public Client GetClientByClientNumber(int number)
        {
            var client = _context.Client.FirstOrDefault(e => e.Client_Number == number);

            return client;
        }


        public User GetUserByClientId(int id)
        {
            var client = _context.Client.Find(id);
            var user = _context.Users.Find(client.UserId);

            return user;
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _context.Client.Select(b => new SelectListItem
            {
                Text = b.FullName + " - " + b.PhoneNumber,
                Value = b.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a client)",
                Value = "0"
            });

            return list;
        }



        /// <summary>
        /// Return a list of all clients that have status Basic or Silver
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<Client> GetAllClientsWithStatusBasicOrSilver()
        {
            var client = _context.Client;
            var historic_status = _context.Historic_Status;
            var notGoldClientList = new List<Client>();

            foreach (var cl in client)
            {
                foreach (var hs in historic_status)
                {
                    if (hs.ClientId == cl.Id)
                    {
                        if (hs.StatusId == 2 || hs.StatusId == 3)
                        {
                            notGoldClientList.Add(cl);
                        }
                    }
                }
            }

            return notGoldClientList;

        }


        /// <summary>
        /// Deducts and updates the clients mileBonus table with the selected amount of miles
        /// </summary>
        /// <param name="milesToPay"></param>
        /// <param name="list"></param>
        public void DeductMilesWithoutCut(int milesToPay, List<Mile_Bonus> list)
        {
            var aux = 0;
            var amountSum = 0;
            List<int> saveID = new List<int>();

            foreach (var item in list)
            {
                if (amountSum < milesToPay)
                {
                    amountSum += item.Miles_Number;
                    aux += 1;
                    saveID.Add(item.Id);
                }
            }

            var diff = amountSum - milesToPay;

            if (diff <= 0)
            {
                //remover as linhas caso a diferença entre valor a pagar e milhas seja igual
                foreach (var item2 in list)
                {
                    foreach (var id in saveID)
                    {
                        if (item2.Id == id)
                        {
                            _context.Mile_Bonus.Remove(item2);                           
                        }
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                //ultimo item a ser tirado a diferença
                var itemToUpdate = _context.Mile_Bonus.Find(saveID.Last());
                itemToUpdate.Miles_Number = diff;

                if (saveID.Count == 1)
                {
                    _context.Mile_Bonus.Update(itemToUpdate);

                    _context.SaveChanges();
                }
                else 
                {
                    _context.Mile_Bonus.Update(itemToUpdate);
                    saveID.Remove(itemToUpdate.Id);

                    foreach (var item2 in list)
                    {
                        foreach (var id in saveID)
                        {
                            if (item2.Id == id)
                            {
                                _context.Mile_Bonus.Remove(item2);

                            }
                        }
                    }

                    _context.SaveChanges();

                }      

                //podia precisar atualizar o available_miles, mas nunca preciso desse valor 

            };           
        }



        /// <summary>
        /// Removes and updates the clients mileBonus table with the selected amount of miles(for extend miles)
        /// </summary>
        /// <param name="milesToPay"></param>
        /// <param name="list"></param>
        public int DeductMilesWitCut(int milesToPay, Client client, List<Mile_Bonus> list)
        {
            var aux = 0;
            var amountSum = 0;
            List<int> saveID = new List<int>();

            foreach (var item in list)
            {
                if (amountSum < milesToPay)
                {
                    amountSum += item.Miles_Number;
                    aux += 1;
                    saveID.Add(item.Id);
                }
            }

            var diff = amountSum - milesToPay;

            if (diff <= 0)
            {
                //remover as linhas caso a diferença entre valor a pagar e milhas seja igual
                foreach (var item2 in list)
                {
                    foreach (var id in saveID)
                    {
                        if (item2.Id == id)
                        {
                            item2.Validity = item2.Validity.AddYears(3);
                        }
                    }
                }
              
                client.AnnualMilesExtended += milesToPay;
                _context.Client.Update(client);

                _context.SaveChanges();
                return diff;
            }
            else
            {
                //ultimo item a ser tirado a diferença
                var itemToUpdate = _context.Mile_Bonus.Find(saveID.Last());
                itemToUpdate.Miles_Number -= diff;
                itemToUpdate.Validity = itemToUpdate.Validity.AddYears(3);

                if (saveID.Count == 1)
                {
                    _context.Mile_Bonus.Update(itemToUpdate);
                    client.Miles_Bonus -= diff;
                    client.AnnualMilesExtended += milesToPay;
                    _context.Client.Update(client);

                    _context.SaveChanges();
                    return diff;
                }
                else
                {
                    _context.Mile_Bonus.Update(itemToUpdate);
                    saveID.Remove(itemToUpdate.Id);

                    foreach (var item2 in list)
                    {
                        foreach (var id in saveID)
                        {
                            if (item2.Id == id)
                            {
                                item2.Validity = item2.Validity.AddYears(3);
                                _context.Mile_Bonus.Update(item2);
                               
                            }
                        }
                    }

                    client.Miles_Bonus -= diff;
                    client.AnnualMilesExtended += milesToPay;
                    _context.Client.Update(client);

                    _context.SaveChanges();
                    return diff;

                }


                //podia precisar atualizar o available_miles, mas nunca preciso desse valor 

            };
        }

        
        /// <summary>
        /// Returns list with amount of values that user can convert based on amount of miles bonus and and anual amount
        /// </summary>
        /// <param name="status"></param>
        /// <param name="client"></param>
        /// <param name="shopList"></param>
        /// <returns></returns>
        public List<BuyMilesShop> ConvertMilesAmountSelection(string status, Client client, List<BuyMilesShop> shopList)
        {
            List<BuyMilesShop> sList = new List<BuyMilesShop>();
            int aux = 0;

            if (status == "Basic" && client.Miles_Status >= 25000)
            {
                foreach (var item in shopList)
                {
                    if (client.AnnualMilesConverted >= 10000)
                    {
                        if (item.MileQuantity <= 10000)
                        {
                            sList.Add(item);
                        }
                    }
                    else
                    {
                        aux = 10000 - client.AnnualMilesTransfered;
                        if (item.MileQuantity <= aux)
                        {
                            sList.Add(item);
                        }
                    }
                }

                return sList;
            }
            else if (status == "Silver" && client.Miles_Status >= 15000 && client.Miles_Status <= 60000)
            {
                foreach (var item in shopList)
                {
                    if (client.AnnualMilesConverted >= 10000)
                    {
                        if (item.MileQuantity <= 10000)
                        {
                            sList.Add(item);
                        }
                        
                    }
                    else
                    {
                        aux = 10000 - client.AnnualMilesBought;
                        if (item.MileQuantity <= aux)
                        {
                            sList.Add(item);
                        }
                    }
                    
                }
                return sList;
            }
            else if (status == "Silver" && client.Miles_Status >= 60000)
            {
                foreach (var item in shopList)
                {
                    aux = 20000 - client.AnnualMilesConverted;
                    if (item.MileQuantity <= aux)
                    {
                        sList.Add(item);
                    }
                }
                return sList;
            }
            else if (status == "Gold" && client.Miles_Status >= 45000)
            {
                foreach (var item in shopList)
                {
                    if (client.AnnualMilesConverted >= 10000)
                    {
                        if (item.MileQuantity <= 10000)
                        {
                            sList.Add(item);
                        }
                    }
                    else
                    {
                        aux = 10000 - client.AnnualMilesTransfered;
                        if (item.MileQuantity <= aux)
                        {
                            sList.Add(item);
                        }
                    }
                }
                return sList;
            }

            return sList;
        }



        public void ConvertMiles(int milesToPay, Client client,List<Mile_Bonus> list)
        {
            var aux = 0;
            var amountSum = 0;
            List<int> saveID = new List<int>();

            foreach (var item in list)
            {
                if (amountSum < milesToPay)
                {
                    amountSum += item.Miles_Number;
                    aux += 1;
                    saveID.Add(item.Id);
                }
            }

            var diff = amountSum - milesToPay;
            var itemToUpdate = _context.Mile_Bonus.Find(saveID.Last());

            if (diff <= 0)
            {
                var mileStatus = new Mile_Status
                {
                    ClientId = client.Id,
                    Miles_Number = (milesToPay / 2),
                    Validity = itemToUpdate.Validity.AddYears(1),
                    available_Miles_Status = client.Miles_Status + (milesToPay / 2),
                };

                client.Miles_Bonus -= milesToPay;
                client.AnnualMilesConverted += milesToPay;
                client.Miles_Status += (milesToPay / 2);

                _context.Mile_Status.Add(mileStatus);
                _context.Client.Update(client);

                //remover as linhas caso a diferença entre valor a pagar e milhas seja igual
                foreach (var item2 in list)
                {
                    foreach (var id in saveID)
                    {
                        if (item2.Id == id)
                        {
                            _context.Mile_Bonus.Remove(item2);                            
                        }
                    }
                }
              
                _context.SaveChanges();
            }
            else
            {
                //ultimo item a ser tirado a diferença;
                itemToUpdate.Miles_Number = diff;

                var mileStatus = new Mile_Status
                {
                    ClientId = client.Id,
                    Miles_Number = (milesToPay / 2),
                    Validity = itemToUpdate.Validity.AddYears(1),
                    available_Miles_Status = client.Miles_Status + (milesToPay / 2),
                };

                client.Miles_Bonus -= milesToPay;
                client.AnnualMilesConverted += milesToPay;
                client.Miles_Status += (milesToPay / 2);

                _context.Mile_Status.Add(mileStatus);
                _context.Client.Update(client);

                if (saveID.Count == 1)
                {
                    _context.Mile_Bonus.Update(itemToUpdate);

                    _context.SaveChanges();
                }
                else
                {
                    _context.Mile_Bonus.Update(itemToUpdate);
                    saveID.Remove(itemToUpdate.Id); //remove from aux list after updating so it doesnt get removed

                    foreach (var item2 in list)
                    {
                        foreach (var id in saveID)
                        {
                            if (item2.Id == id)
                            {
                                _context.Mile_Bonus.Remove(item2);

                            }
                        }
                    }

                    _context.SaveChanges();

                }

                //podia precisar atualizar o available_miles, mas nunca preciso desse valor 

            };
        }
    }
}
