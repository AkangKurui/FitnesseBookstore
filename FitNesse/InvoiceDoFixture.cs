using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using fit;
using FitNesse.models;
using FitNesse.param;

namespace FitNesse
{
    public class InvoiceDoFixture : fitlibrary.DoFixture
    {
        public InvoiceReport invoice;
        public string invoiceId;
        public string itemId;
        public List<ItemReport> items;
        public ItemReport item;

        public InvoiceDoFixture()
        {
            items = new List<ItemReport>();
        }

        #region Invoice
        public string InvoiceId()
        {
            return invoiceId;
        }

        public void BuatInvoiceDenganPelanggan(string customer)
        {
            var data = new CreateInvoiceParam
            {
                Name = customer
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PostAsJsonAsync("api/invoice", data).Result;
            invoiceId = response.Content.ReadAsAsync<string>().Result;
        }

        public void AmbilInvoice()
        {
            HttpClient client = SetUri();
            SetRequestHeader(client);

            var responseGet = client.GetAsync("api/invoice/" + this.InvoiceId()).Result;
            invoice = responseGet.Content.ReadAsAsync<InvoiceReport>().Result;
        }

        public bool TanggalInvoiceSamaDenganHariIni()
        {
            var date = DateTime.Today;
            var dateNow = date.ToString("dd-MM-yyyy");
            var dateOut = invoice.TransactionDate.ToString("dd-MM-yyyy");
            return dateNow.Equals(dateOut);
        }

        public bool TanggalBerlakuDitambahTermin()
        {
            var date = DateTime.Today;
            var dateOut = date.AddDays(invoice.Customer.Term.Value).ToString("dd-MM-yyyy");
            return dateOut.Equals(invoice.DueDate.ToString("dd-MM-yyyy"));
        }

        public string TanggalBerlakuSetelahTanggalTransaksiDiUbah()
        {
            return invoice.DueDate.ToString("dd-MM-yyyy");
        }

        public string UbahTanggalInvoiceMenjadiTanggalInvoiceSekarangAdalah(string date)
        {
            var transDate = new ChangeDateParam
            {
                date = date
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PutAsJsonAsync("api/invoice/" + this.InvoiceId(), transDate).Result;
            invoice = response.Content.ReadAsAsync<InvoiceReport>().Result;
            return invoice.TransactionDate.ToString("dd-MM-yyyy");
        }

        public double TotalInvoice()
        {
            return invoice.NetTotal;
        }
        #endregion

        #region Items
        public string ItemId()
        {
            return item.ItemId.ToString();
        }

        public void AmbilItemDariInvoice()
        {
            HttpClient client = SetUri();
            SetRequestHeader(client);

            var responseGet = client.GetAsync("api/invoice/" + this.InvoiceId() + "/item/" + this.ItemId()).Result;
            var content = responseGet.Content.ReadAsStringAsync().Result;
            item = responseGet.Content.ReadAsAsync<ItemReport>().Result;
        }

        public int TotalItems()
        {
            return items.Count();
        }

        public void TambahItemDenganBarcodeQtyPrice(string name, string barcode, int qty, double price)
        {
            var data = new AddItemParam
            {
                Name = name,
                Barcode = barcode,
                Price = price,
                Qty = qty
            };
            HttpClient client = SetUri();
            SetRequestHeader(client);
            var response = client.PostAsJsonAsync("api/invoice/" + invoiceId, data).Result;
            item = response.Content.ReadAsAsync<ItemReport>().Result;
            items.Add(item);
        }

        public void UbahQty(int qty)
        {
            var data = new ChangeQtyParam
            {
                qty = qty
            };

            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.PutAsJsonAsync("api/invoice/" + this.InvoiceId() + "/item/" + this.ItemId() + "?qty="+qty,"").Result;
            var itemResponse = response.Content.ReadAsAsync<ItemReport>().Result;
            var itemUpdate = items.Where(x => x.ItemId.Equals(itemResponse.ItemId)).SingleOrDefault();
            itemUpdate.Qty = itemResponse.Qty;
            itemUpdate.Total = itemResponse.Total;
        }
        #endregion

        #region Private
        private static void SetRequestHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static HttpClient SetUri()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://bookstore.dokuku.net/");
            return client;
        }

        private InvoiceReport getInvoiceId(string invoiceId)
        {
            HttpClient client = SetUri();
            SetRequestHeader(client);

            var response = client.GetAsync("api/invoice/" + invoiceId).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            invoice = response.Content.ReadAsAsync<InvoiceReport>().Result;
            return invoice;
        }
        #endregion
    }
}
