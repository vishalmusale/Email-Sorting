using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace Email
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            // Initialize Column Names in GridViewTable
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Email";
            dataGridView1.Columns[2].Name = "Domain";
            dataGridView1.Columns[3].Name = "Extension";

            // Initialize Drop-down list of Combobox
            string[] sortList = new string[] { "-", "Email Address", "Domain Name", "Extension" };
            comboBox1.Items.AddRange(sortList);
            comboBox1.SelectedIndex = 0;
        }

       
        String[,] table;
        List<String> tableList = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Text Files";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            String text = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String MyReader = openFileDialog1.FileName;
                // Read the file as one string.
                text = System.IO.File.ReadAllText(MyReader);
            }
            
            getEmail(text);
        }

        private void getEmail(String txtFile)
        {
            //throw new NotImplementedException();
            
            string text = txtFile;

            String emailList = text;
            // To get an idividual email from the text file...
            List<string> emails = text.Split(';').ToList<string>();
                        
            // Sort Emails
            emails.Sort();
           
            // list to hash set
            List<string> uniqueEmails = new List<string>(makeUnique(emails).ToList());
            

            table = new String[uniqueEmails.Count, 4];
            
            for (int i=0; i< uniqueEmails.Count; i++)
            {
                String e = uniqueEmails[i];
                if (uniqueEmails[i].Length > 5)  // To check if anything is present or not after ';'
                {   // ex. a@b.c = 5 char atleast
                    
                    // for sorting 
                    tableList.Add(i.ToString());
                    tableList.Add(uniqueEmails[i]);

                    // To split the email into parts

                    // To get Domain and Extension
                    // to get 2nd part of the email = student.csulb.edu
                    List<string> secndPart = uniqueEmails[i].Split('@').ToList<string>();

                    String secondPart = secndPart[1];

                    String ext = "", company = "";
                    for (int f = 0, r = secondPart.Length - 1; f <= r - 1; f++)
                    {
                        // Ex. 	vishal.musale@student.csulb.edu;musale.vishal@outlook.com
                        // To get com / edu/ in
                        if (secondPart[r] != '.')
                        {
                            ext = secondPart[r] + ext;
                            r--;
                        }

                        // To get "student.csulb"
                        company = company + secondPart[f];

                    }
                    
                    tableList.Add(company);
                    tableList.Add(ext);
                }
            }

            showDataGridView(tableList);    // To show the entire table
            
        }
        

        private HashSet<string> makeUnique(List<string> emails)
        {
            // list to hash set
            HashSet<String> uniqueEmails = new HashSet<string>(emails);

            return uniqueEmails;
        }

        
        private void emailSplitter(String email)
        {
            // To get Domain and Extension
            // to get 2nd part of the email = student.csulb.edu
            List<string> secndPart = email.Split('@').ToList<string>();

            String secondPart = secndPart[1];
            
            String ext = "", company = "";
            for (int f = 0, r = secondPart.Length - 1; f <= r - 1; f++)
            {
                // Ex. 	vishal.musale@student.csulb.edu;musale.vishal@outlook.com
                // To get com / edu/ in
                if (secondPart[r] != '.')
                {
                    ext = secondPart[r] + ext;
                    r--;
                }

                // To get "student.csulb"
                company = company + secondPart[f];

            }            
        }

        
        private void populateDataGridTable_ExtSort()
        {
            String[] temp = new String[4];  // To create temperary string for swapping data...
            
            // Copy into temp list so that, we can not messup with the original list
            List<string> extSortList = new List<string>(tableList);     
            
            for (int i = 0; i < extSortList.Count; i=i+4)
            {
                for (int j = i+4; j < extSortList.Count; j=j+4)
                {
                    if (extSortList[i+3].CompareTo(extSortList[j+3]) > 0)
                    {
                        temp[0] = extSortList[i];
                        temp[1] = extSortList[i+1];
                        temp[2] = extSortList[i+2];
                        temp[3] = extSortList[i+3];

                        extSortList[i] = extSortList[j];
                        extSortList[i+1] = extSortList[j+1];
                        extSortList[i+2] = extSortList[j+2];
                        extSortList[i+3] = extSortList[j+3];

                        extSortList[j] = temp[0];
                        extSortList[j+1] = temp[1];
                        extSortList[j+2] = temp[2];
                        extSortList[j+3] = temp[3];
                    }
                }
            }
            
            showDataGridView(extSortList);
        }
       
        private void populateDataGridTable_DomainSort()
        {
            String[] temp = new String[4];  // To create temperary string for swapping data...

            // Copy into temp list so that, we can not messup with the original list
            List<string> domainSortList = new List<string>(tableList);

            for (int i = 0; i < domainSortList.Count; i = i + 4)
            {
                for (int j = i + 4; j < domainSortList.Count; j = j + 4)
                {
                    if (domainSortList[i + 2].CompareTo(domainSortList[j + 2]) > 0)
                    {
                        temp[0] = domainSortList[i];
                        temp[1] = domainSortList[i + 1];
                        temp[2] = domainSortList[i + 2];
                        temp[3] = domainSortList[i + 3];

                        domainSortList[i] = domainSortList[j];
                        domainSortList[i + 1] = domainSortList[j + 1];
                        domainSortList[i + 2] = domainSortList[j + 2];
                        domainSortList[i + 3] = domainSortList[j + 3];

                        domainSortList[j] = temp[0];
                        domainSortList[j + 1] = temp[1];
                        domainSortList[j + 2] = temp[2];
                        domainSortList[j + 3] = temp[3];
                    }
                }
            }

            showDataGridView(domainSortList);
        }

        
        private void makeDataGridViewEmpty()
        {
            int rowCount = dataGridView1.RowCount;
            if (rowCount != 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            else
            {

            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    makeDataGridViewEmpty();
                    showDataGridView(tableList);
                    break;
                case 1:
                    if (dataGridView1.RowCount <= 1)
                    {
                        System.Windows.Forms.MessageBox.Show("Please Import a Text File.");
                    }
                    makeDataGridViewEmpty();
                    showDataGridView(tableList);
                    break;
                case 2:
                    if (dataGridView1.RowCount <= 1)
                    {
                        System.Windows.Forms.MessageBox.Show("Please Import a Text File.");
                    }
                    makeDataGridViewEmpty();
                    populateDataGridTable_DomainSort();
                    break;
                case 3:
                    if (dataGridView1.RowCount <= 1)
                    {
                        System.Windows.Forms.MessageBox.Show("Please Import a Text File.");
                    }
                    makeDataGridViewEmpty();
                    populateDataGridTable_ExtSort();
                    break;
                default:
                    makeDataGridViewEmpty();
                    showDataGridView(tableList);
                    break;
            }
            
        }

        // Showing DataGridTable
        private void showDataGridView(List<String> temp)
        {
            for (int i = 0; i < tableList.Count; i = i + 4)
            {
                dataGridView1.Rows.Add(new object[] {
                    temp[i], temp[i+1], temp[i+2], temp[i+3]
                });
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    // To store selected rows into hashset
                    var set = new List<string>();
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        set.Add(Convert.ToString(r.Cells[1].Value));
                    }

                    //set.Sort();
                    // Displays a SaveFileDialog so the user can save the 2 Text files
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Text File | *.txt";
                    saveFileDialog1.Title = "Save an Text File";
                    saveFileDialog1.ShowDialog();

                    

                    //System.IO.StreamWriter column = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))

                    // If the file name is not an empty string open it for saving.
                    if (saveFileDialog1.FileName != "")
                    {

                        // Create another file to show emails columns
                        String tempColumnFilePath = "";
                        tempColumnFilePath = saveFileDialog1.FileName;
                        string fname;
                        fname = Path.GetFileName(tempColumnFilePath);
                        String ColumnFilePath = tempColumnFilePath.Replace(fname, "");
                        ColumnFilePath = ColumnFilePath + Path.GetFileNameWithoutExtension(tempColumnFilePath) + "-column.txt";
                        
                        if (File.Exists(ColumnFilePath))
                        {
                            using (System.IO.StreamWriter column = new System.IO.StreamWriter(ColumnFilePath))
                            {
                                for (int i = set.Count-1; i>= 0; i--)
                                {
                                    column.WriteLine(set[i]);
                                }
                            }
                        }
                        else
                        {
                            ColumnFilePath = ColumnFilePath.Replace(".txt", DateTime.Now.ToString("yyyyMMddTHHmmss")+ ".txt");
                            using (System.IO.StreamWriter column = new System.IO.StreamWriter(ColumnFilePath))
                            {
                                for (int i = set.Count - 1; i >= 0; i--)
                                {
                                    column.Write(set[i]);
                                    column.WriteLine(";");
                                }
                            }
                        }
                        
                        // Saves the text via a FileStream created by the OpenFile method.
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.OpenFile()))
                        {
                            for (int i = set.Count - 1; i >= 0; i--)
                            {
                                file.Write(set[i]);
                                file.Write(";");                              
                            }
                        }
                        
                    }
                    
                }
                else
                    System.Windows.Forms.MessageBox.Show("Please Select Rows");
            }
            else
                System.Windows.Forms.MessageBox.Show("Please Import a Text File.");
        }
    }
}
