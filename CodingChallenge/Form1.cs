using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodingChallenge
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateSortedList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateSortedList();
        }


        public void LoadDatagrid(List<Client> SortedList)
        {
            dataGridView1.DataSource = SortedList;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = true;


            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "Name";
            column2.HeaderText = "Name";
            column2.DataPropertyName = "Name";
            dataGridView1.Columns.Add(column2);


           

            
        }


        public void GenerateSortedList()
        {
            //Load Clients file
            string[] lines = System.IO.File.ReadAllLines(textBox1.Text);
            //List of Clients
            List<Client> Clients = new List<Client>();
            //Insert each Client into the list
            foreach (string line in lines)
            {
                float rate;
                float NR;
                float NC;
                using (Client cliente = new Client())
                {
                    //Parse Data
                    String[] LineSplit = line.Split('|');
                    cliente.ID = Convert.ToInt32(LineSplit[0]);
                    cliente.Name = LineSplit[1];
                    cliente.Lastname = LineSplit[2];
                    cliente.CurrentRole = LineSplit[3];
                    cliente.Country = LineSplit[4];
                    cliente.Industry = LineSplit[5];
                    cliente.NRecommendations = Convert.ToInt32(LineSplit[6]);
                    cliente.NConnections = Convert.ToInt32(LineSplit[7]);
                    NR = cliente.NRecommendations;
                    NC = cliente.NConnections;
                    //Calculate Rate
                    if (cliente.NRecommendations == 0) NR = 0.5F;
                    if (cliente.NConnections == 0) NC = 0.5F;
                    cliente.Rate = NR * NC;
                    //Add client to List
                    Clients.Add(cliente);
                }
            }

            List<Client> SortedList = Clients.OrderByDescending(o => o.Rate).ToList();

            //Display list in Datagridview
            LoadDatagrid(Clients);
           // Use string builder for better performance at creating the output file
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                using (Client cliente = SortedList[i])
                {
                    sb.AppendLine(cliente.ID.ToString());
                }
            }

            //Write output file 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(textBox2.Text))
            {
                file.WriteLine(sb.ToString()); 
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines(@"people.in");
        }
    }


    // Create a class for the object Client
    public class Client : IDisposable
    {
        public int ID;
        public string Name;
        public string Lastname;
        public string CurrentRole;
        public string Country;
        public string Industry;
        public int NRecommendations;
        public int NConnections;
        public float Rate;
        bool disposed;


        // Manage Dispose method for better performance 
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
