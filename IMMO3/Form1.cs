using System;
using System.Drawing;
using System.Windows.Forms;

namespace IMMO3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
               
        string[] positions = new string[] { "111", "110", "101", "100", "011", "010", "001", "000" }; // поиск позиций по правилам
        char[] cellRules; // число правил
        bool start = true;        
        int rowCounter = 0; // кол-во строк
              
        private char[] acceptRules(int rule)
        {
            char[] result;

            string binaryCode = Convert.ToString(rule, 2);

            int binaryLength = binaryCode.Length;
            if (binaryLength != 8)
            {
                for (int i = 0; i < 8 - binaryLength; i++)
                {
                    binaryCode = "0" + binaryCode;
                }
            }

            result = binaryCode.ToCharArray();

            return result;
        }

        private char calculateLayerCellValue(char[] xyz)
        {
            char result;

            string code = new string(xyz);

            int index = Array.IndexOf(positions, code);

            result = cellRules[index];

            return result;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {            
            nud_Columns.Enabled = false; 
            nud_Rules.Enabled = false;        
            btn_Start.Enabled = true;         

            dataGridView.Rows.Clear();       // Очищается вся таблица
                     
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns.RemoveAt(0);
            }
                  
            for (int i = 0; i < (int)nud_Columns.Value; i++) // добавление колонок
            {
                dataGridView.Columns.Add("", "");
            }
                       
            dataGridView.Rows.Add(); // первая - базовая строка
                   
            int rule = (int)nud_Rules.Value;
            cellRules = acceptRules(rule);
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {            
            btn_Create.Enabled = false;  
            btn_Stop.Enabled = true;    
            btn_Start.Enabled = false;   

            start = false;              

            rowCounter = 0;
            timer1.Start();             // Запуск программы
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {            
            timer1.Stop();                      
            btn_Create.Enabled = true;           
            btn_Stop.Enabled = false;            
            start = true;                      
            nud_Columns.Enabled = true;     
            nud_Rules.Enabled = true;
        }
              
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (start)
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.DarkGreen;
                        dataGridView.ClearSelection();
                        break;
                    case MouseButtons.Right:
                        dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                        dataGridView.ClearSelection();
                        break;
                }
        }
                
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }
             
        private void timer1_Tick(object sender, EventArgs e)
        {           
            char[] previousLayer = new char[dataGridView.Columns.Count]; // прошлое поколение
                        
            char[] currentLayer = new char[dataGridView.Columns.Count]; // следующее поколение 
                        
            char[] xyz = new char[3];

            
            for (int i = 0; i < previousLayer.Length; i++) // заполнение предыдущих значениями 
            {
                if (dataGridView[i, rowCounter].Style.BackColor == Color.DarkGreen) previousLayer[i] = '1';
                else previousLayer[i] = '0';
            }
           
            dataGridView.Rows.Add(); // новая строка
            rowCounter++;
                       
            for (int i = 0; i < currentLayer.Length; i++) // вычисление значения нового поколения
            {
                xyz[0] = previousLayer[(i + previousLayer.Length - 1) % previousLayer.Length];
                xyz[1] = previousLayer[i];
                xyz[2] = previousLayer[(i + previousLayer.Length + 1) % previousLayer.Length];
                currentLayer[i] = calculateLayerCellValue(xyz);
            }
                      
            for (int i = 0; i < currentLayer.Length; i++)
            {
                if (currentLayer[i] == '0') dataGridView[i, rowCounter].Style.BackColor = Color.White;
                else dataGridView[i, rowCounter].Style.BackColor = Color.DarkGreen;
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Start_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}