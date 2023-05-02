using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace OOP_lab4._2
{
    public partial class Form1 : Form
    {
        private Model _model; //создание модели
        public Form1() 
        {
            InitializeComponent();
            _model = new Model(); //создаем новую модель
            _model.Observers += new System.EventHandler(this.Update); //запускается функция 
        }
        
        private void Update(object sender, EventArgs e) //перенос сохраненных данных 
        {
            textBoxA.Text = _model.GetValueA().ToString();
            numericUpDownA.Value = _model.GetValueA(); 
            trackBarA.Value = _model.GetValueA();

            textBoxB.Text = _model.GetValueB().ToString();
            numericUpDownB.Value = _model.GetValueB();
            trackBarB.Value = _model.GetValueB();

            textBoxC.Text = _model.GetValueC().ToString();
            numericUpDownC.Value = _model.GetValueC();
            trackBarC.Value = _model.GetValueC();
        }
        // Передаем значения в модель
        // TextBox
        private void textBoxA_TextChanged(object sender, EventArgs e) 
        {
            if (textBoxA.Text == String.Empty) //если ничего не ввели, то 0
                _model.SetValueA(0);
            else
                _model.SetValueA(Int32.Parse(textBoxA.Text));//в ином случае переданное число сохраняем
        }

        private void textBoxB_TextChanged(object sender, EventArgs e)
        {
            timer1.Start(); //таймер на изменение B
        }

        private void textBoxC_TextChanged(object sender, EventArgs e)
        {
            if (textBoxC.Text == String.Empty)
                _model.SetValueC(0);
            else
                _model.SetValueC(Int32.Parse(textBoxC.Text));
        }
        
        // NumericUpDown
        private void numericUpDownA_ValueChanged(object sender, EventArgs e)
        {
            _model.SetValueA(Decimal.ToInt32(numericUpDownA.Value));//запись значения в модель
        }
        private void numericUpDownB_ValueChanged(object sender, EventArgs e)
        {
            _model.SetValueB(Decimal.ToInt32(numericUpDownB.Value));
        }
        private void numericUpDownC_ValueChanged(object sender, EventArgs e)
        {
            _model.SetValueC(Decimal.ToInt32(numericUpDownC.Value));
        }

        // TrackBar
        private void trackBarA_Scroll(object sender, EventArgs e)
        {
            _model.SetValueA(trackBarA.Value);//запись значений в модель
        }
        private void trackBarB_Scroll(object sender, EventArgs e)
        {
            _model.SetValueB(trackBarB.Value);
        }
        private void trackBarC_Scroll(object sender, EventArgs e)
        { 
            _model.SetValueC(trackBarC.Value);
        }

        // Сохранение значений 
        private void Form1_Load(object sender, EventArgs e)
        {
            _model.Observers.Invoke(this, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) 
            //сохранение данных при закрытие формы(приложения)
        {
            Properties.Settings.Default.Value1 = _model.GetValueA();
            Properties.Settings.Default.Value2 = _model.GetValueB();
            Properties.Settings.Default.Value3 = _model.GetValueC();
            Properties.Settings.Default.Save();
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            if (textBoxB.Text == String.Empty)
                _model.SetValueB(0);
            else
                _model.SetValueB(Int32.Parse(textBoxB.Text));
        }
    }

    class Model
    {
        private int[] _value = new int[3] { 0, 0, 0 }; //массив со значениями А,В,С
        public System.EventHandler Observers;
        
        public Model()
        {
            _value[0] = Properties.Settings.Default.Value1; //сохр. параметры
            _value[1] = Properties.Settings.Default.Value2;
            _value[2] = Properties.Settings.Default.Value3;
        }
        
        public void SetValueA(int value) //сохранение значения А
        {
            _value[0] = value;
            if (_value[0] > _value[1])
                _value[1] = _value[0];
            if (_value[0] > _value[2])
                _value[2] = _value[0];
            
            Observers.Invoke(this, null); //обновляет данные на экране
        }
        public void SetValueB(int value)
        {
            int tmp = _value[1];
            _value[1] = value;
            if (!(_value[0] <= _value[1] && _value[1] <= _value[2])) //подходит ли значение под условие
                _value[1] = tmp; //иначе возвращает первоначальное значение
            
            Observers.Invoke(this, null);
        }
        public void SetValueC(int value)
        {
            _value[2] = value;
            if (_value[2] < _value[1])
                _value[1] = _value[2];
            if (_value[2] < _value[0])
                _value[0] = _value[2];
            
            Observers.Invoke(this, null);
        }
        
        public int GetValueA()
        {
            return _value[0];
        }
        public int GetValueB()
        {
            return _value[1];
        }
        public int GetValueC()
        {
            return _value[2];
        }
    }
}