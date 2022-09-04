namespace FiledVisualzerDemo
{
    partial class DemoForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.RectanglesSetDemo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RectanglesSetDemo
            // 
            this.RectanglesSetDemo.Location = new System.Drawing.Point(29, 22);
            this.RectanglesSetDemo.Name = "RectanglesSetDemo";
            this.RectanglesSetDemo.Size = new System.Drawing.Size(167, 23);
            this.RectanglesSetDemo.TabIndex = 0;
            this.RectanglesSetDemo.Text = "RectanglesSetDemo";
            this.RectanglesSetDemo.UseVisualStyleBackColor = true;
            this.RectanglesSetDemo.Click += new System.EventHandler(this.RectanglesSetDemo_Click);
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RectanglesSetDemo);
            this.Name = "DemoForm";
            this.Text = "FieldVisualizerDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RectanglesSetDemo;
    }
}

