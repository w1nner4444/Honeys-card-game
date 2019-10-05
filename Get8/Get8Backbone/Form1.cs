using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Get8Backbone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Trade_Button_Click(object sender, EventArgs e)
        {
            Trade_Window trade = new Trade_Window();
            trade.ShowDialog();
        }

        private void Card_Table_Update(object sender, EventArgs e)
        {
            int x, y;
            x = 17;
            y = 2;
            int i, j;
            for(i=0; i<=this.Card_Table.RowCount; i++)
            {
                for(j=0; j<=this.Card_Table.ColumnCount; j++)
                {
                    Control c = this.Card_Table.GetControlFromPosition(i, j);
                    Label label = c as Label;
                    if(label != null)
                    {
                        if (j == 1) 
                        {
                            label.Text = "Amount of said card";
                        }
                        //add player card count
                        else 
                        {
                            label.Text = "The card that goes here";
                            //I recommend putting a list to 
                            //iterate with for this part
                        }//card for this row
                    }
                    
                    
                }
            }
        }

        private void Turn_Table_Update(object sender, EventArgs e)
        {
            Control c = this.Turn_Table.GetControlFromPosition(0, 0);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
