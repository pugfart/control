namespace Control
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.connect_bt = new System.Windows.Forms.Button();
            this.IPaddress = new System.Windows.Forms.TextBox();
            this.disconnect_bt = new System.Windows.Forms.Button();
            this.J1plus = new System.Windows.Forms.Button();
            this.J1minus = new System.Windows.Forms.Button();
            this.J2plus = new System.Windows.Forms.Button();
            this.J2minus = new System.Windows.Forms.Button();
            this.J3plus = new System.Windows.Forms.Button();
            this.J3minus = new System.Windows.Forms.Button();
            this.J4plus = new System.Windows.Forms.Button();
            this.J4minus = new System.Windows.Forms.Button();
            this.J5plus = new System.Windows.Forms.Button();
            this.J5minus = new System.Windows.Forms.Button();
            this.J6plus = new System.Windows.Forms.Button();
            this.J6minus = new System.Windows.Forms.Button();
            this.Xplus = new System.Windows.Forms.Button();
            this.Xminus = new System.Windows.Forms.Button();
            this.Yplus = new System.Windows.Forms.Button();
            this.Yminus = new System.Windows.Forms.Button();
            this.Zplus = new System.Windows.Forms.Button();
            this.Zminus = new System.Windows.Forms.Button();
            this.RXplus = new System.Windows.Forms.Button();
            this.RXminus = new System.Windows.Forms.Button();
            this.RYplus = new System.Windows.Forms.Button();
            this.RYminus = new System.Windows.Forms.Button();
            this.RZplus = new System.Windows.Forms.Button();
            this.RZminus = new System.Windows.Forms.Button();
            this.Speed = new System.Windows.Forms.TrackBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Movement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.J6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start_thread = new System.Windows.Forms.Button();
            this.stop_thread = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.showjoints = new System.Windows.Forms.Label();
            this.showaxisposition = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.MoveJ = new System.Windows.Forms.Button();
            this.MoveL = new System.Windows.Forms.Button();
            this.startthread = new System.Windows.Forms.Button();
            this.stopthread = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // connect_bt
            // 
            this.connect_bt.Location = new System.Drawing.Point(31, 29);
            this.connect_bt.Name = "connect_bt";
            this.connect_bt.Size = new System.Drawing.Size(88, 23);
            this.connect_bt.TabIndex = 0;
            this.connect_bt.Text = "Connect";
            this.connect_bt.UseVisualStyleBackColor = true;
            // 
            // IPaddress
            // 
            this.IPaddress.Location = new System.Drawing.Point(136, 26);
            this.IPaddress.Name = "IPaddress";
            this.IPaddress.Size = new System.Drawing.Size(122, 25);
            this.IPaddress.TabIndex = 1;
            this.IPaddress.Text = "輸入手臂IP位址";
            // 
            // disconnect_bt
            // 
            this.disconnect_bt.Location = new System.Drawing.Point(31, 68);
            this.disconnect_bt.Name = "disconnect_bt";
            this.disconnect_bt.Size = new System.Drawing.Size(88, 23);
            this.disconnect_bt.TabIndex = 2;
            this.disconnect_bt.Text = "Disconnect";
            this.disconnect_bt.UseVisualStyleBackColor = true;
            // 
            // J1plus
            // 
            this.J1plus.Location = new System.Drawing.Point(20, 466);
            this.J1plus.Name = "J1plus";
            this.J1plus.Size = new System.Drawing.Size(75, 23);
            this.J1plus.TabIndex = 3;
            this.J1plus.Text = "J1+";
            this.J1plus.UseVisualStyleBackColor = true;
            // 
            // J1minus
            // 
            this.J1minus.Location = new System.Drawing.Point(121, 466);
            this.J1minus.Name = "J1minus";
            this.J1minus.Size = new System.Drawing.Size(75, 23);
            this.J1minus.TabIndex = 4;
            this.J1minus.Text = "J1-";
            this.J1minus.UseVisualStyleBackColor = true;
            // 
            // J2plus
            // 
            this.J2plus.Location = new System.Drawing.Point(20, 514);
            this.J2plus.Name = "J2plus";
            this.J2plus.Size = new System.Drawing.Size(75, 23);
            this.J2plus.TabIndex = 5;
            this.J2plus.Text = "J2+";
            this.J2plus.UseVisualStyleBackColor = true;
            // 
            // J2minus
            // 
            this.J2minus.Location = new System.Drawing.Point(121, 514);
            this.J2minus.Name = "J2minus";
            this.J2minus.Size = new System.Drawing.Size(75, 23);
            this.J2minus.TabIndex = 6;
            this.J2minus.Text = "J2-";
            this.J2minus.UseVisualStyleBackColor = true;
            // 
            // J3plus
            // 
            this.J3plus.Location = new System.Drawing.Point(20, 562);
            this.J3plus.Name = "J3plus";
            this.J3plus.Size = new System.Drawing.Size(75, 23);
            this.J3plus.TabIndex = 7;
            this.J3plus.Text = "J3+";
            this.J3plus.UseVisualStyleBackColor = true;
            // 
            // J3minus
            // 
            this.J3minus.Location = new System.Drawing.Point(121, 562);
            this.J3minus.Name = "J3minus";
            this.J3minus.Size = new System.Drawing.Size(75, 23);
            this.J3minus.TabIndex = 8;
            this.J3minus.Text = "J3-";
            this.J3minus.UseVisualStyleBackColor = true;
            // 
            // J4plus
            // 
            this.J4plus.Location = new System.Drawing.Point(20, 610);
            this.J4plus.Name = "J4plus";
            this.J4plus.Size = new System.Drawing.Size(75, 23);
            this.J4plus.TabIndex = 9;
            this.J4plus.Text = "J4+";
            this.J4plus.UseVisualStyleBackColor = true;
            // 
            // J4minus
            // 
            this.J4minus.Location = new System.Drawing.Point(121, 610);
            this.J4minus.Name = "J4minus";
            this.J4minus.Size = new System.Drawing.Size(75, 23);
            this.J4minus.TabIndex = 10;
            this.J4minus.Text = "J4-";
            this.J4minus.UseVisualStyleBackColor = true;
            // 
            // J5plus
            // 
            this.J5plus.Location = new System.Drawing.Point(20, 658);
            this.J5plus.Name = "J5plus";
            this.J5plus.Size = new System.Drawing.Size(75, 23);
            this.J5plus.TabIndex = 11;
            this.J5plus.Text = "J5+";
            this.J5plus.UseVisualStyleBackColor = true;
            // 
            // J5minus
            // 
            this.J5minus.Location = new System.Drawing.Point(121, 658);
            this.J5minus.Name = "J5minus";
            this.J5minus.Size = new System.Drawing.Size(75, 23);
            this.J5minus.TabIndex = 12;
            this.J5minus.Text = "J5-";
            this.J5minus.UseVisualStyleBackColor = true;
            // 
            // J6plus
            // 
            this.J6plus.Location = new System.Drawing.Point(20, 706);
            this.J6plus.Name = "J6plus";
            this.J6plus.Size = new System.Drawing.Size(75, 23);
            this.J6plus.TabIndex = 13;
            this.J6plus.Text = "J6+";
            this.J6plus.UseVisualStyleBackColor = true;
            // 
            // J6minus
            // 
            this.J6minus.Location = new System.Drawing.Point(121, 706);
            this.J6minus.Name = "J6minus";
            this.J6minus.Size = new System.Drawing.Size(75, 23);
            this.J6minus.TabIndex = 14;
            this.J6minus.Text = "J6-";
            this.J6minus.UseVisualStyleBackColor = true;
            // 
            // Xplus
            // 
            this.Xplus.Location = new System.Drawing.Point(222, 466);
            this.Xplus.Name = "Xplus";
            this.Xplus.Size = new System.Drawing.Size(75, 23);
            this.Xplus.TabIndex = 15;
            this.Xplus.Text = "X+";
            this.Xplus.UseVisualStyleBackColor = true;
            // 
            // Xminus
            // 
            this.Xminus.Location = new System.Drawing.Point(323, 466);
            this.Xminus.Name = "Xminus";
            this.Xminus.Size = new System.Drawing.Size(75, 23);
            this.Xminus.TabIndex = 16;
            this.Xminus.Text = "X-";
            this.Xminus.UseVisualStyleBackColor = true;
            // 
            // Yplus
            // 
            this.Yplus.Location = new System.Drawing.Point(222, 514);
            this.Yplus.Name = "Yplus";
            this.Yplus.Size = new System.Drawing.Size(75, 23);
            this.Yplus.TabIndex = 17;
            this.Yplus.Text = "Y+";
            this.Yplus.UseVisualStyleBackColor = true;
            // 
            // Yminus
            // 
            this.Yminus.Location = new System.Drawing.Point(323, 514);
            this.Yminus.Name = "Yminus";
            this.Yminus.Size = new System.Drawing.Size(75, 23);
            this.Yminus.TabIndex = 18;
            this.Yminus.Text = "Y-";
            this.Yminus.UseVisualStyleBackColor = true;
            // 
            // Zplus
            // 
            this.Zplus.Location = new System.Drawing.Point(222, 562);
            this.Zplus.Name = "Zplus";
            this.Zplus.Size = new System.Drawing.Size(75, 23);
            this.Zplus.TabIndex = 19;
            this.Zplus.Text = "Z+";
            this.Zplus.UseVisualStyleBackColor = true;
            // 
            // Zminus
            // 
            this.Zminus.Location = new System.Drawing.Point(323, 562);
            this.Zminus.Name = "Zminus";
            this.Zminus.Size = new System.Drawing.Size(75, 23);
            this.Zminus.TabIndex = 20;
            this.Zminus.Text = "Z-";
            this.Zminus.UseVisualStyleBackColor = true;
            // 
            // RXplus
            // 
            this.RXplus.Location = new System.Drawing.Point(222, 610);
            this.RXplus.Name = "RXplus";
            this.RXplus.Size = new System.Drawing.Size(75, 23);
            this.RXplus.TabIndex = 21;
            this.RXplus.Text = "Rx+";
            this.RXplus.UseVisualStyleBackColor = true;
            // 
            // RXminus
            // 
            this.RXminus.Location = new System.Drawing.Point(323, 610);
            this.RXminus.Name = "RXminus";
            this.RXminus.Size = new System.Drawing.Size(75, 23);
            this.RXminus.TabIndex = 22;
            this.RXminus.Text = "Rx-";
            this.RXminus.UseVisualStyleBackColor = true;
            // 
            // RYplus
            // 
            this.RYplus.Location = new System.Drawing.Point(222, 658);
            this.RYplus.Name = "RYplus";
            this.RYplus.Size = new System.Drawing.Size(75, 23);
            this.RYplus.TabIndex = 23;
            this.RYplus.Text = "Ry+";
            this.RYplus.UseVisualStyleBackColor = true;
            // 
            // RYminus
            // 
            this.RYminus.Location = new System.Drawing.Point(323, 658);
            this.RYminus.Name = "RYminus";
            this.RYminus.Size = new System.Drawing.Size(75, 23);
            this.RYminus.TabIndex = 24;
            this.RYminus.Text = "Ry-";
            this.RYminus.UseVisualStyleBackColor = true;
            // 
            // RZplus
            // 
            this.RZplus.Location = new System.Drawing.Point(222, 706);
            this.RZplus.Name = "RZplus";
            this.RZplus.Size = new System.Drawing.Size(75, 23);
            this.RZplus.TabIndex = 25;
            this.RZplus.Text = "Rz+";
            this.RZplus.UseVisualStyleBackColor = true;
            // 
            // RZminus
            // 
            this.RZminus.Location = new System.Drawing.Point(323, 706);
            this.RZminus.Name = "RZminus";
            this.RZminus.Size = new System.Drawing.Size(75, 23);
            this.RZminus.TabIndex = 26;
            this.RZminus.Text = "Rz-";
            this.RZminus.UseVisualStyleBackColor = true;
            // 
            // Speed
            // 
            this.Speed.Location = new System.Drawing.Point(404, 464);
            this.Speed.Name = "Speed";
            this.Speed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Speed.Size = new System.Drawing.Size(56, 263);
            this.Speed.TabIndex = 27;
            this.Speed.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Movement,
            this.J1,
            this.J2,
            this.J3,
            this.J4,
            this.J5,
            this.J6,
            this.X,
            this.Y,
            this.Z,
            this.Rx,
            this.Ry,
            this.Rz});
            this.dataGridView1.Location = new System.Drawing.Point(20, 113);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1348, 337);
            this.dataGridView1.TabIndex = 28;
            // 
            // Movement
            // 
            this.Movement.HeaderText = "Movement";
            this.Movement.Name = "Movement";
            // 
            // J1
            // 
            this.J1.HeaderText = "J1";
            this.J1.Name = "J1";
            // 
            // J2
            // 
            this.J2.HeaderText = "J2";
            this.J2.Name = "J2";
            // 
            // J3
            // 
            this.J3.HeaderText = "J3";
            this.J3.Name = "J3";
            // 
            // J4
            // 
            this.J4.HeaderText = "J4";
            this.J4.Name = "J4";
            // 
            // J5
            // 
            this.J5.HeaderText = "J5";
            this.J5.Name = "J5";
            // 
            // J6
            // 
            this.J6.HeaderText = "J6";
            this.J6.Name = "J6";
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            // 
            // Z
            // 
            this.Z.HeaderText = "Z";
            this.Z.Name = "Z";
            // 
            // Rx
            // 
            this.Rx.HeaderText = "Rx";
            this.Rx.Name = "Rx";
            // 
            // Ry
            // 
            this.Ry.HeaderText = "Ry";
            this.Ry.Name = "Ry";
            // 
            // Rz
            // 
            this.Rz.HeaderText = "Rz";
            this.Rz.Name = "Rz";
            // 
            // start_thread
            // 
            this.start_thread.Location = new System.Drawing.Point(136, 68);
            this.start_thread.Name = "start_thread";
            this.start_thread.Size = new System.Drawing.Size(97, 23);
            this.start_thread.TabIndex = 29;
            this.start_thread.Text = "Start Thread";
            this.start_thread.UseVisualStyleBackColor = true;
            // 
            // stop_thread
            // 
            this.stop_thread.Location = new System.Drawing.Point(250, 68);
            this.stop_thread.Name = "stop_thread";
            this.stop_thread.Size = new System.Drawing.Size(97, 23);
            this.stop_thread.TabIndex = 30;
            this.stop_thread.Text = "Stop Thread";
            this.stop_thread.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(632, 497);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 25);
            this.textBox1.TabIndex = 31;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(632, 702);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 25);
            this.textBox2.TabIndex = 32;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(632, 661);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 25);
            this.textBox3.TabIndex = 33;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(632, 620);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 25);
            this.textBox4.TabIndex = 34;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(632, 579);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 25);
            this.textBox5.TabIndex = 35;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(632, 538);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 25);
            this.textBox6.TabIndex = 36;
            // 
            // showjoints
            // 
            this.showjoints.AutoSize = true;
            this.showjoints.Location = new System.Drawing.Point(632, 466);
            this.showjoints.Name = "showjoints";
            this.showjoints.Size = new System.Drawing.Size(67, 15);
            this.showjoints.TabIndex = 37;
            this.showjoints.Text = "各軸角度";
            // 
            // showaxisposition
            // 
            this.showaxisposition.AutoSize = true;
            this.showaxisposition.Location = new System.Drawing.Point(761, 466);
            this.showaxisposition.Name = "showaxisposition";
            this.showaxisposition.Size = new System.Drawing.Size(67, 15);
            this.showaxisposition.TabIndex = 44;
            this.showaxisposition.Text = "立體座標";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(761, 538);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 25);
            this.textBox7.TabIndex = 43;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(761, 579);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 25);
            this.textBox8.TabIndex = 42;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(761, 620);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 25);
            this.textBox9.TabIndex = 41;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(761, 661);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(100, 25);
            this.textBox10.TabIndex = 40;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(761, 702);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(100, 25);
            this.textBox11.TabIndex = 39;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(761, 497);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(100, 25);
            this.textBox12.TabIndex = 38;
            // 
            // MoveJ
            // 
            this.MoveJ.Location = new System.Drawing.Point(499, 466);
            this.MoveJ.Name = "MoveJ";
            this.MoveJ.Size = new System.Drawing.Size(75, 23);
            this.MoveJ.TabIndex = 45;
            this.MoveJ.Text = "MoveJ";
            this.MoveJ.UseVisualStyleBackColor = true;
            // 
            // MoveL
            // 
            this.MoveL.Location = new System.Drawing.Point(499, 514);
            this.MoveL.Name = "MoveL";
            this.MoveL.Size = new System.Drawing.Size(75, 23);
            this.MoveL.TabIndex = 46;
            this.MoveL.Text = "MoveL";
            this.MoveL.UseVisualStyleBackColor = true;
            // 
            // startthread
            // 
            this.startthread.Location = new System.Drawing.Point(894, 68);
            this.startthread.Name = "startthread";
            this.startthread.Size = new System.Drawing.Size(75, 23);
            this.startthread.TabIndex = 47;
            this.startthread.Text = "執行";
            this.startthread.UseVisualStyleBackColor = true;
            // 
            // stopthread
            // 
            this.stopthread.Location = new System.Drawing.Point(975, 68);
            this.stopthread.Name = "stopthread";
            this.stopthread.Size = new System.Drawing.Size(75, 23);
            this.stopthread.TabIndex = 48;
            this.stopthread.Text = "停止";
            this.stopthread.UseVisualStyleBackColor = true;
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(1056, 68);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 49;
            this.clear.Text = "清空";
            this.clear.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 750);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.stopthread);
            this.Controls.Add(this.startthread);
            this.Controls.Add(this.MoveL);
            this.Controls.Add(this.MoveJ);
            this.Controls.Add(this.showaxisposition);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.showjoints);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.stop_thread);
            this.Controls.Add(this.start_thread);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Speed);
            this.Controls.Add(this.RZminus);
            this.Controls.Add(this.RZplus);
            this.Controls.Add(this.RYminus);
            this.Controls.Add(this.RYplus);
            this.Controls.Add(this.RXminus);
            this.Controls.Add(this.RXplus);
            this.Controls.Add(this.Zminus);
            this.Controls.Add(this.Zplus);
            this.Controls.Add(this.Yminus);
            this.Controls.Add(this.Yplus);
            this.Controls.Add(this.Xminus);
            this.Controls.Add(this.Xplus);
            this.Controls.Add(this.J6minus);
            this.Controls.Add(this.J6plus);
            this.Controls.Add(this.J5minus);
            this.Controls.Add(this.J5plus);
            this.Controls.Add(this.J4minus);
            this.Controls.Add(this.J4plus);
            this.Controls.Add(this.J3minus);
            this.Controls.Add(this.J3plus);
            this.Controls.Add(this.J2minus);
            this.Controls.Add(this.J2plus);
            this.Controls.Add(this.J1minus);
            this.Controls.Add(this.J1plus);
            this.Controls.Add(this.disconnect_bt);
            this.Controls.Add(this.IPaddress);
            this.Controls.Add(this.connect_bt);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_bt;
        private System.Windows.Forms.TextBox IPaddress;
        private System.Windows.Forms.Button disconnect_bt;
        private System.Windows.Forms.Button J1plus;
        private System.Windows.Forms.Button J1minus;
        private System.Windows.Forms.Button J2plus;
        private System.Windows.Forms.Button J2minus;
        private System.Windows.Forms.Button J3plus;
        private System.Windows.Forms.Button J3minus;
        private System.Windows.Forms.Button J4plus;
        private System.Windows.Forms.Button J4minus;
        private System.Windows.Forms.Button J5plus;
        private System.Windows.Forms.Button J5minus;
        private System.Windows.Forms.Button J6plus;
        private System.Windows.Forms.Button J6minus;
        private System.Windows.Forms.Button Xplus;
        private System.Windows.Forms.Button Xminus;
        private System.Windows.Forms.Button Yplus;
        private System.Windows.Forms.Button Yminus;
        private System.Windows.Forms.Button Zplus;
        private System.Windows.Forms.Button Zminus;
        private System.Windows.Forms.Button RXplus;
        private System.Windows.Forms.Button RXminus;
        private System.Windows.Forms.Button RYplus;
        private System.Windows.Forms.Button RYminus;
        private System.Windows.Forms.Button RZplus;
        private System.Windows.Forms.Button RZminus;
        private System.Windows.Forms.TrackBar Speed;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Movement;
        private System.Windows.Forms.DataGridViewTextBoxColumn J1;
        private System.Windows.Forms.DataGridViewTextBoxColumn J2;
        private System.Windows.Forms.DataGridViewTextBoxColumn J3;
        private System.Windows.Forms.DataGridViewTextBoxColumn J4;
        private System.Windows.Forms.DataGridViewTextBoxColumn J5;
        private System.Windows.Forms.DataGridViewTextBoxColumn J6;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Z;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ry;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rz;
        private System.Windows.Forms.Button start_thread;
        private System.Windows.Forms.Button stop_thread;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label showjoints;
        private System.Windows.Forms.Label showaxisposition;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Button MoveJ;
        private System.Windows.Forms.Button MoveL;
        private System.Windows.Forms.Button startthread;
        private System.Windows.Forms.Button stopthread;
        private System.Windows.Forms.Button clear;
    }
}

