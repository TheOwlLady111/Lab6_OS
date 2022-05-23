using System.Timers;
namespace Lab6_OS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //InitializeTimer();
        }

        private int _sizeOfMemory;
        private int _sizeOfSegment;
        private int _ammountOfRows;
        private int _ammountOfSegments;
        private List<int> matrix = new List<int>();
        private List<(int, int)> cellsList= new List<(int, int)>();
        private Process _process;
        private Color _baseColor = Color.White;
        private Color _processColor = Color.BlueViolet;
        private Color _adressColor = Color.LawnGreen;
        System.Windows.Forms.Timer Timer1 = new System.Windows.Forms.Timer();

        private void radioButton1_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 16;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 32;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 64;
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 128;
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 256;
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            _sizeOfMemory = 512;
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            _sizeOfSegment = 1;
        }

        private void radioButton8_Click(object sender, EventArgs e)
        {
            _sizeOfSegment = 2;
        }

        private void radioButton9_Click(object sender, EventArgs e)
        {
            _sizeOfSegment = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            _ammountOfRows = _sizeOfMemory / _sizeOfSegment;
            for (int i=1; i<=_ammountOfRows; i++)
            {
                dataGridView1.Rows.Add(i.ToString(), string.Empty, 0);
                matrix.Add(0); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
                _process = new Process();
                _process.Size = Convert.ToInt32(textBox1.Text);
                _ammountOfSegments = _process.GetSegments(_sizeOfSegment);
                int start = 0;
                int end = -1;
                for (int i = 0; i < _ammountOfSegments; i++)
                {
                    if (_ammountOfSegments - 1 == i && _process.Size % _sizeOfSegment > 0)
                    {
                            int n = _sizeOfSegment - _process.Size % _sizeOfSegment;
                            end = end  + n * 1024;
                    }
                    else
                    { end = end + 1024 * _sizeOfSegment; }
                    string cells = start.ToString() + ".." + end.ToString();   
                    dataGridView1.Rows[i].SetValues((i + 1).ToString(), cells, 1);
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = _processColor;
                    matrix[i] = 1;
                    cellsList.Add((start, end));
                    start = end + 1;
                    
                }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _ammountOfSegments; i++)
            {
                dataGridView1.Rows[i].SetValues((i + 1).ToString(), string.Empty, 0);
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = _baseColor;
                matrix[i] = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int from = Convert.ToInt32(textBox2.Text);
            int to = Convert.ToInt32(textBox3.Text);
            int n1 = 0, n2 = 0;
            for (int i = 0; i < _ammountOfSegments; i++)
            {
                if (from >= cellsList[i].Item1 && from <= cellsList[i].Item2)
                {
                    n1 = i;
                }
                else if (to >= cellsList[i].Item1 && to <= cellsList[i].Item2)
                {
                    n2 = i;
                }
            }
            // Set the caption to the current time.  
            dataGridView1.Rows[n1].SetValues((n1 + 1).ToString(), "From " + from.ToString(), 1);
            dataGridView1.Rows[n1].DefaultCellStyle.BackColor = _adressColor;

            dataGridView1.Rows[n2].SetValues((n2 + 1).ToString(), "To " + to.ToString(), 1);
            dataGridView1.Rows[n2].DefaultCellStyle.BackColor = _adressColor;
            InitializeTimer();
            Timer1.Enabled = true;

        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            int from = Convert.ToInt32(textBox2.Text);
            int to = Convert.ToInt32(textBox3.Text);
            int n1 = 0, n2 = 0;
            for (int i = 0; i < _ammountOfSegments; i++)
            {
                if (from >= cellsList[i].Item1 && from <= cellsList[i].Item2)
                {
                    n1 = i;
                }
                else if (to >= cellsList[i].Item1 && to <= cellsList[i].Item2)
                {
                    n2 = i;
                }
            }
            // Set the caption to the current time.  
            dataGridView1.Rows[n1].SetValues((n1 + 1).ToString(), cellsList[n1].Item1 + ".." + cellsList[n1].Item2, 1);
            dataGridView1.Rows[n1].DefaultCellStyle.BackColor = _processColor;

            dataGridView1.Rows[n2].SetValues((n2 + 1).ToString(), cellsList[n2].Item1 + ".." + cellsList[n2].Item2, 1);
            dataGridView1.Rows[n2].DefaultCellStyle.BackColor = _processColor;
        }

        private void InitializeTimer()
        {
              
           
            Timer1.Interval = 5000;
            Timer1.Tick += new EventHandler(Timer1_Tick);
            
            Timer1.Enabled = true;

            button4.Click += new EventHandler(button4_Click);
        }

        private void FindingTheLessSuitable(Process process)
        {
            int amount = process.GetSegments(_sizeOfSegment);
            int start = 0, start1 = 0, end1 = 0, end = 0, size = 0, size1 = 0; 
            for (int i=1; i<matrix.Count-1; i++)
            {
                if (matrix[i] == 0 && matrix[i-1] == 1)
                {
                    start = 1;
                    size++;
                }
                else if (matrix[i] == 0)
                {
                    size++;
                }
                else if (matrix[i] == 0 && matrix[i+1] == 1)
                {
                    size++;
                    end = i;
                    if (size > size1)
                    {
                        start1 = start;
                        end1 = end;
                    }
                    size = 0;

                }
            }
        }
    }
}