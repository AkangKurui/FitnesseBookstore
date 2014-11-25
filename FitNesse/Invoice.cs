using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using FitNesse.param;
using System.Net.Http.Headers;
using FitNesse.models;

namespace FitNesse
{
    public class Invoice : fit.ColumnFixture
    {
        public string invoiceId;
        public string customer;
        public string date;
        public string itemName;
        public string barcode;
        public int qty;
        public double price;
        public string itemId;
        public InvoiceReport invoice;

        public string CreateInvoice()
        {
            var data = new CreateInvoiceParam
            {
                Name = customer
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PostAsJsonAsync("api/invoice", data).Result;
            var content = response.Content.ReadAsAsync<string>().Result;
            return content;
        }

        public string ChangeDate()
        {
            var transDate = new ChangeDateParam
            {
                date = date
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PutAsJsonAsync("api/invoice/" + invoiceId, transDate).Result;
            return response.StatusCode.ToString();
        }

        public string GetList()
        {
            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.GetAsync("api/invoice").Result;
            return response.StatusCode.ToString();
        }

        public string GetInvoiceById()
        {
            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.GetAsync("api/invoice/" + invoiceId).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            invoice = response.Content.ReadAsAsync<InvoiceReport>().Result;
            return invoice.ToString();
        }

        public string TanggalTransaksi()
        { 
            return invoice.TransactionDate.ToString("dd-MM-yyyy");
        }

        public string DeleteInvoice()
        {
            var data = new CreateInvoiceParam
            {
                Name = customer
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.DeleteAsync("api/invoice/" + invoiceId).Result;
            return response.StatusCode.ToString();
        }

        public string AddItem()
        {
            var data = new AddItemParam
            {
                Name = itemName,
                Barcode = barcode,
                Price = price,
                Qty = qty
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PostAsJsonAsync("api/invoice/" + invoiceId, data).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            return content;
        }

        public string ChangeQty()
        {
            var data = new ChangeQtyParam
            {
                qty = qty
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PutAsJsonAsync("api/invoice/" + invoiceId + "/" + itemId + "?qty=", qty).Result;
            return response.StatusCode.ToString();
        }

        #region Private
        private static void SetRequestHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static HttpClient SetUri()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/api-bookstore/");
            return client;
        }
        #endregion
    }
}
