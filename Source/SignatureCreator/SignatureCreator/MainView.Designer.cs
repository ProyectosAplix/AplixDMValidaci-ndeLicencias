using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace SignatureCreator
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private bool isInDisEncrypterMode = false;
        private bool isInEncrypterMode = true;
        private bool isInXMLGeneratorMode = false;
        private bool isInSentenceSQLMode = false;
        private bool Disencrypter = false;
        private string FinalState = "";
        private string FinalLicenseType = "";
        private string Firm = "";
        private string FirmDisencrypter = "";



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Configura la visibilidad y estado de los controles relacionados con el modo de desencriptación.
        /// </summary>
        /// <param name="visible">Indica si los controles deben ser visibles o no.</param>
        /// <remarks>
        /// Cuando los controles son visibles, se ajustan las propiedades de visibilidad de varios TextBox y Label.
        /// Si la firma (Firm) no está vacía y el modo de desencriptación (Disencrypter) está activado, 
        /// se muestra la firma desencriptada (FirmDisencrypter) en el TextBox correspondiente.
        /// </remarks>
        private void SetControlsForDisEncrypter(bool visible)
        {
            isInDisEncrypterMode = visible; //validación de pantalla
            txtDisFirm.Visible = visible;
            lbDisFirm.Visible = visible;
            txtDisKey.Visible = visible;
            lbDisKey.Visible = visible;
            if (!string.IsNullOrWhiteSpace(Firm) && visible == true && Disencrypter == true)
            {
                txtDisResult.Text = FirmDisencrypter;
                txtDisResult.Visible = true;
                txtDisResult.Enabled = false;
                lbDisResult.Visible = true;


            }
            else
            {
                txtDisResult.Visible = false;
                lbDisResult.Visible = false;
            }
        }


        /// <summary>
        /// Configura la visibilidad y estado de los controles relacionados con la generación de XML.
        /// </summary>
        /// <param name="visible">Indica si los controles deben ser visibles o no.</param>
        /// <remarks>
        /// Cuando los controles son visibles, se ajustan las propiedades de visibilidad de varios TextBox, Label y DateTimePicker.
        /// Se establece el texto del TextBox correspondiente con la firma actual (Firm).
        /// </remarks>
        private void SetControlsForXMLGenerator(bool visible)
        {
            isInXMLGeneratorMode = visible; //validación de pantalla
            txtXMLFirm.Visible = visible;
            txtXMLFirm.Text = Firm;
            lbXMLFirm.Visible = visible;
            timeDateLine.Visible = visible;
            lbDateline.Visible = visible;
            timeDateLine.Enabled = false;
            txtXMLID.Visible = visible;
            lbXMLID.Visible = visible;


        }


        /// <summary>
        /// Configura la visibilidad y estado de los controles relacionados con la generación de sentencias SQL.
        /// </summary>
        /// <param name="visible">Indica si los controles deben ser visibles o no.</param>
        /// <remarks>
        /// Cuando los controles son visibles, se ajusta la visibilidad del RichTextBox y se establece un texto predeterminado
        /// que representa una sentencia SQL para generar un identificador único basado en propiedades del servidor.
        /// </remarks>
        private void SetControlsForSentenceSQL(bool visible)
        {
            isInSentenceSQLMode = visible; //validación de pantalla
            this.richTextSentenceSQL.Text = "DECLARE @UniqueIdentifier NVARCHAR(100)\r\nSET @UniqueIdentifier = @@SERVERNAME + '_' + CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(50)) + '_aplix'\r\n\r\nSELECT @UniqueIdentifier AS UniqueIdentifier";
            richTextSentenceSQL.Visible = visible;

        }


        /// <summary>
        /// Configura la visibilidad y estado de los controles relacionados con la encriptación de licencias.
        /// </summary>
        /// <param name="visible">Indica si los controles deben ser visibles o no.</param>
        /// <remarks>
        /// Cuando los controles son visibles, se ajustan las propiedades de visibilidad de varios TextBox, Label y DateTimePicker.
        /// Si existe una firma (Firm) y los controles son visibles, se establece el texto del TextBox de resultado (txtResult) con la firma actual.
        /// </remarks>
        private void SetControlsForEncrypter(bool visible)
        {
            isInEncrypterMode = visible; //validación de pantalla
            cmbState.Visible = visible;
            lbState.Visible = visible;
            timeDateLine.Visible = visible;
            timeDateLine.Enabled = true;
            lbDateline.Visible = visible;
            txtServerID.Visible = visible;
            lbServerID.Visible = visible;
            cmbTypeLicense.Visible = visible;
            lbTypeLicense.Visible = visible;
            txtKey.Visible = visible;
            lbKey.Visible = visible;

            if (!string.IsNullOrWhiteSpace(Firm) && visible == true)
            {
                txtResult.Text = Firm;
                txtResult.Visible = true;
                txtResult.Enabled = false;
                lbResult.Visible = true;


            }
            else
            {
                txtResult.Visible = false;
                lbResult.Visible = false;
            }

        }

        /// <summary>
        /// Maneja el evento de clic para el botón "Desencriptar".
        /// </summary>
        /// <param name="sender">El objeto que genera el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        /// <remarks>
        /// Si no está en modo de desencriptación, configura la visibilidad de los controles para desactivar los controles de encriptación,
        /// sentencias SQL y generación de XML, y activa los controles de desencriptación. Si está en modo de desencriptación, llama al
        /// método DisencrypterFirm para realizar la desencriptación.
        /// </remarks>
        private void btnDisEncrypter_Click(object sender, EventArgs e)
        {
            if (!isInDisEncrypterMode)
            {

                SetControlsForEncrypter(false);
                SetControlsForSentenceSQL(false);
                SetControlsForXMLGenerator(false);
                SetControlsForDisEncrypter(true);
            }
            else
            {
                DisencrypterFirm();
            }


        }


        /// <summary>
        /// Maneja el evento de clic para el botón "Encriptar".
        /// </summary>
        /// <param name="sender">El objeto que genera el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        /// <remarks>
        /// Si no está en modo de encriptación, configura la visibilidad de los controles para desactivar los controles de desencriptación,
        /// generación de XML y sentencias SQL, y activa los controles de encriptación. Si está en modo de encriptación, llama al
        /// método FirmGenerator para realizar la encriptación.
        /// </remarks>
        private void btnEncrypter_Click(object sender, EventArgs e)
        {

            if (!isInEncrypterMode)
            {
                SetControlsForDisEncrypter(false);
                SetControlsForXMLGenerator(false);
                SetControlsForSentenceSQL(false);
                SetControlsForEncrypter(true);
            }
            else
            {
                FirmGenerator();
            }


        }


        /// <summary>
        /// Maneja el evento de clic para el botón "Generar XML".
        /// </summary>
        /// <param name="sender">El objeto que genera el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        /// <remarks>
        /// Si no está en modo de generación de XML y la firma no está vacía, configura la visibilidad de los controles para desactivar
        /// los controles de desencriptación, encriptación y sentencias SQL, y activa los controles de generación de XML. Si está en modo
        /// de generación de XML, llama al método XMLGenerator para generar el archivo XML. Si la firma está vacía, muestra un mensaje
        /// de advertencia.
        /// </remarks>
        private void btnXMLGenerator_Click(object sender, EventArgs e)
        {

            if (!isInXMLGeneratorMode && !string.IsNullOrWhiteSpace(Firm))
            {
                SetControlsForDisEncrypter(false);
                SetControlsForEncrypter(false);
                SetControlsForSentenceSQL(false);
                SetControlsForXMLGenerator(true);
            }
            else
            {

                if (string.IsNullOrWhiteSpace(Firm))
                {
                    MessageBox.Show("Se debe crear una firma primero", "Firma Vacia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    XMLGenerator();
                }

            }


        }


        /// <summary>
        /// Maneja el evento de clic para el botón "Generar Sentencia SQL".
        /// </summary>
        /// <param name="sender">El objeto que genera el evento.</param>
        /// <param name="e">Los datos del evento.</param>
        /// <remarks>
        /// Si no está en modo de sentencias SQL, configura la visibilidad de los controles para desactivar los controles de desencriptación,
        /// encriptación y generación de XML, y activa los controles de sentencias SQL.
        /// </remarks>
        private void btnSentenceSQL_Click(object sender, EventArgs e)
        {

            if (!isInSentenceSQLMode)
            {
                SetControlsForDisEncrypter(false);
                SetControlsForEncrypter(false);
                SetControlsForXMLGenerator(false);
                SetControlsForSentenceSQL(true);
            }

        }


        /// <summary>
        /// Genera la firma encriptada utilizando los datos proporcionados y muestra el resultado en el cuadro de texto correspondiente.
        /// </summary>
        /// <remarks>
        /// La función toma los valores de los controles de la interfaz de usuario, como el estado, la fecha de vencimiento, el ID del servidor,
        /// el tipo de licencia y la clave. Luego, verifica que todos los campos estén completos y que la longitud de la clave sea mayor o igual a dos.
        /// Si se cumplen estas condiciones, crea la firma combinando los valores y la encripta utilizando la función FEncrypter. 
        /// Muestra el resultado en el cuadro de texto correspondiente y proporciona mensajes de éxito o error mediante cuadros de mensaje.
        /// </remarks>
        private void FirmGenerator()
        {

            string state = FinalState;
            string dateline = timeDateLine.Text;
            string serverID = txtServerID.Text;
            string typeLicense = FinalLicenseType;
            string key = txtKey.Text;


            if (string.IsNullOrWhiteSpace(state) ||
                string.IsNullOrWhiteSpace(dateline) ||
                string.IsNullOrWhiteSpace(serverID) ||
                string.IsNullOrWhiteSpace(typeLicense) ||
                string.IsNullOrWhiteSpace(key))
            {

                MessageBox.Show("Todos los campos deben ser completados.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                if (key.Length >= 2)
                {
                    string firm = state + "|" + dateline + "|" + serverID + "|" + typeLicense;
                    if (FEncrypter(firm, key))
                    {
                        Firm = Encryptacion.Encrypter(firm, key);
                        MessageBox.Show("Firma creada correctamente.", "Firma Creada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtResult.Text = Firm;
                        txtResult.Visible = true;
                        txtResult.Enabled = false;
                        lbResult.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo crear la firma.", "Firma No Creada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    MessageBox.Show("La key tiene que ser mayor a tamaño dos", "Error con key", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }


        /// <summary>
        /// Desencripta la firma utilizando los datos proporcionados y muestra el resultado en el cuadro de texto correspondiente.
        /// </summary>
        /// <remarks>
        /// La función toma los valores de los controles de la interfaz de usuario, como la firma y la clave. 
        /// Verifica que ambos campos estén completos. Si se cumplen estas condiciones, intenta desencriptar la firma utilizando la función FDisEncrypter. 
        /// Si la desencriptación es exitosa, muestra el resultado en el cuadro de texto correspondiente y proporciona un mensaje de éxito mediante un cuadro de mensaje.
        /// </remarks>
        private void DisencrypterFirm()
        {
            string firm = txtDisFirm.Text;
            string key = txtDisKey.Text;

            if (string.IsNullOrWhiteSpace(firm) ||
                        string.IsNullOrWhiteSpace(key))
            {

                MessageBox.Show("Todos los campos deben ser completados.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // No continúes con el procesamiento si hay campos vacíos
            }
            else
            {
                if (FDisEncrypter(firm, key))
                {
                    MessageBox.Show("Firma desencriptada correctamente.", "Firma Desencriptada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDisResult.Text = FirmDisencrypter;
                    txtDisResult.Visible = true;
                    txtDisResult.Enabled = false;
                    lbDisResult.Visible = true;
                    Disencrypter = true;
                }

            }

        }


        /// <summary>
        /// Genera un archivo XML con la información proporcionada y muestra un mensaje con la ubicación del archivo.
        /// </summary>
        /// <remarks>
        /// La función intenta generar un archivo XML en el escritorio del usuario con la información proporcionada, incluyendo el estado, ID, fecha de vencimiento y firma.
        /// Si la generación es exitosa, muestra un mensaje indicando la ubicación del archivo. En caso de error, muestra un mensaje de error con detalles.
        /// </remarks>
        private void XMLGenerator()
        {
            try
            {

                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktop, "Licencia.xml");


                using (XmlWriter writer = XmlWriter.Create(filePath))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("Licencia");
                    writer.WriteStartElement("Estado");
                    writer.WriteString(FinalState);
                    writer.WriteEndElement();
                    writer.WriteStartElement("ID");
                    writer.WriteString(txtXMLID.Text);
                    writer.WriteEndElement();
                    writer.WriteStartElement("FechaVencimiento");
                    writer.WriteString(timeDateLine.Text);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Firma");
                    writer.WriteString(txtXMLFirm.Text);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                MessageBox.Show("Se ha generado el archivo XML en el desktop: " + filePath);
                Console.WriteLine("Se ha generado el archivo XML en el desktop: " + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el archivo XML: " + ex.Message);
                Console.WriteLine("Error al generar el archivo XML: " + ex.Message);
            }

        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnEncrypter = new System.Windows.Forms.ToolStripButton();
            this.SeparatorBtnDisEncrypter = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisEncrypter = new System.Windows.Forms.ToolStripButton();
            this.SeparatorBtnXMLGenerator = new System.Windows.Forms.ToolStripSeparator();
            this.btnXMLGenerator = new System.Windows.Forms.ToolStripButton();
            this.SeparatorBtnSentenceSQL = new System.Windows.Forms.ToolStripSeparator();
            this.btnSentenceSQL = new System.Windows.Forms.ToolStripButton();
            this.panel = new System.Windows.Forms.Panel();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.lbState = new System.Windows.Forms.Label();
            this.timeDateLine = new System.Windows.Forms.DateTimePicker();
            this.lbDateline = new System.Windows.Forms.Label();
            this.txtServerID = new System.Windows.Forms.TextBox();
            this.lbServerID = new System.Windows.Forms.Label();
            this.cmbTypeLicense = new System.Windows.Forms.ComboBox();
            this.lbTypeLicense = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lbKey = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lbResult = new System.Windows.Forms.Label();
            this.txtDisFirm = new System.Windows.Forms.TextBox();
            this.lbDisFirm = new System.Windows.Forms.Label();
            this.txtDisKey = new System.Windows.Forms.TextBox();
            this.lbDisKey = new System.Windows.Forms.Label();
            this.txtDisResult = new System.Windows.Forms.TextBox();
            this.lbDisResult = new System.Windows.Forms.Label();
            this.txtXMLFirm = new System.Windows.Forms.TextBox();
            this.lbXMLFirm = new System.Windows.Forms.Label();
            this.txtXMLID = new System.Windows.Forms.TextBox();
            this.lbXMLID = new System.Windows.Forms.Label();
            this.richTextSentenceSQL = new System.Windows.Forms.RichTextBox();

            this.toolStrip.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEncrypter,
            this.SeparatorBtnDisEncrypter,
            this.btnDisEncrypter,
            this.SeparatorBtnXMLGenerator,
            this.btnXMLGenerator,
            this.SeparatorBtnSentenceSQL,
            this.btnSentenceSQL});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(380, 27);
            this.toolStrip.TabIndex = 5;
            this.toolStrip.Text = "toolStrip";
            // 
            // btnEncrypter
            // 
            this.btnEncrypter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEncrypter.Image = global::SignatureCreator.Properties.Resources.Encrypter;
            this.btnEncrypter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEncrypter.Name = "btnEncrypter";
            this.btnEncrypter.Size = new System.Drawing.Size(24, 24);
            this.btnEncrypter.Text = "Encriptar";
            this.btnEncrypter.Click += new System.EventHandler(this.btnEncrypter_Click);
            // 
            // SeparatorBtnDisEncrypter
            // 
            this.SeparatorBtnDisEncrypter.Name = "SeparatorBtnDisEncrypter";
            this.SeparatorBtnDisEncrypter.Size = new System.Drawing.Size(6, 27);
            // 
            // btnDisEncrypter
            // 
            this.btnDisEncrypter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDisEncrypter.Image = global::SignatureCreator.Properties.Resources.DisEncrypter;
            this.btnDisEncrypter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisEncrypter.Name = "btnDisEncrypter";
            this.btnDisEncrypter.Size = new System.Drawing.Size(24, 24);
            this.btnDisEncrypter.Text = "Desencriptar";
            this.btnDisEncrypter.Click += new System.EventHandler(this.btnDisEncrypter_Click);
            // 
            // SeparatorBtnXMLGenerator
            // 
            this.SeparatorBtnXMLGenerator.Name = "SeparatorBtnXMLGenerator";
            this.SeparatorBtnXMLGenerator.Size = new System.Drawing.Size(6, 27);
            // 
            // btnXMLGenerator
            // 
            this.btnXMLGenerator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnXMLGenerator.Image = global::SignatureCreator.Properties.Resources.XML;
            this.btnXMLGenerator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnXMLGenerator.Name = "btnXMLGenerator";
            this.btnXMLGenerator.Size = new System.Drawing.Size(24, 24);
            this.btnXMLGenerator.Text = "XML";
            this.btnXMLGenerator.Click += new System.EventHandler(this.btnXMLGenerator_Click);
            // 
            // SeparatorBtnSqlSentence
            // 
            this.SeparatorBtnSentenceSQL.Name = "SeparatorBtnSentenceSQL";
            this.SeparatorBtnSentenceSQL.Size = new System.Drawing.Size(6, 27);
            // 
            // btnSentenceSQL
            // 
            this.btnSentenceSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSentenceSQL.Image = global::SignatureCreator.Properties.Resources.SQL;
            this.btnSentenceSQL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSentenceSQL.Name = "btnSentenceSQL";
            this.btnSentenceSQL.Size = new System.Drawing.Size(24, 24);
            this.btnSentenceSQL.Text = "SQL";
            this.btnSentenceSQL.Click += new System.EventHandler(this.btnSentenceSQL_Click);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.cmbState);
            this.panel.Controls.Add(this.lbState);
            this.panel.Controls.Add(this.timeDateLine);
            this.panel.Controls.Add(this.lbDateline);
            this.panel.Controls.Add(this.txtServerID);
            this.panel.Controls.Add(this.lbServerID);
            this.panel.Controls.Add(this.cmbTypeLicense);
            this.panel.Controls.Add(this.lbTypeLicense);
            this.panel.Controls.Add(this.txtKey);
            this.panel.Controls.Add(this.lbKey);
            this.panel.Controls.Add(this.txtDisFirm);
            this.panel.Controls.Add(this.lbDisFirm);
            this.panel.Controls.Add(this.txtDisKey);
            this.panel.Controls.Add(this.lbDisKey);
            this.panel.Controls.Add(this.txtXMLFirm);
            this.panel.Controls.Add(this.lbXMLFirm);
            this.panel.Controls.Add(this.txtXMLID);
            this.panel.Controls.Add(this.lbXMLID);
            this.panel.Controls.Add(this.txtResult);
            this.panel.Controls.Add(this.lbResult);
            this.panel.Controls.Add(this.txtDisResult);
            this.panel.Controls.Add(this.lbDisResult);
            this.panel.Controls.Add(this.richTextSentenceSQL);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(0, 27);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(380, 221);
            this.panel.TabIndex = 8;
            // 
            // cmbState
            // 
            cmbState.Location = new System.Drawing.Point(102, 11);
            cmbState.Name = "cmbState";
            cmbState.Size = new System.Drawing.Size(262, 20);
            cmbState.TabIndex = 0;
            cmbState.Items.Add("Activo");
            cmbState.Items.Add("Inactivo");
            cmbState.Tag = new string[] { "A", "I" };

            // Manejar el evento de selección cambiada
            cmbState.SelectedIndexChanged += (sender, e) =>
            {

                string[] internalValues = (string[])cmbState.Tag;


                if (cmbState.SelectedIndex >= 0 && cmbState.SelectedIndex < internalValues.Length)
                {

                    string selectedInternalValue = internalValues[cmbState.SelectedIndex];


                    Console.WriteLine("Valor interno seleccionado: " + selectedInternalValue);
                    this.FinalState = selectedInternalValue;
                }
            };

            // 
            // lbState
            // 
            this.lbState.AutoSize = true;
            this.lbState.Location = new System.Drawing.Point(5, 15);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(43, 13);
            this.lbState.TabIndex = 0;
            this.lbState.Text = "Estado:";
            // 
            // timeDateLine
            // 
            this.timeDateLine.CustomFormat = "dd-MM-yyyy";
            this.timeDateLine.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeDateLine.Location = new System.Drawing.Point(102, 40);
            this.timeDateLine.Name = "timeDateLine";
            this.timeDateLine.Size = new System.Drawing.Size(262, 20);
            this.timeDateLine.TabIndex = 1;
            // 
            // lbDateline
            // 
            this.lbDateline.AutoSize = true;
            this.lbDateline.Location = new System.Drawing.Point(5, 44);
            this.lbDateline.Name = "lbDateline";
            this.lbDateline.Size = new System.Drawing.Size(98, 13);
            this.lbDateline.TabIndex = 1;
            this.lbDateline.Text = "FechaVencimiento:";
            // 
            // txtServerID
            // 
            this.txtServerID.Location = new System.Drawing.Point(102, 70);
            this.txtServerID.Name = "txtServerID";
            this.txtServerID.Size = new System.Drawing.Size(262, 20);
            this.txtServerID.TabIndex = 2;
            // 
            // lbServerID
            // 
            this.lbServerID.AutoSize = true;
            this.lbServerID.Location = new System.Drawing.Point(5, 74);
            this.lbServerID.Name = "lbServerID";
            this.lbServerID.Size = new System.Drawing.Size(60, 13);
            this.lbServerID.TabIndex = 2;
            this.lbServerID.Text = "IDServidor:";
            // 
            // cmbTypeLicense
            // 
            this.cmbTypeLicense.Location = new System.Drawing.Point(102, 100);
            this.cmbTypeLicense.Name = "cmbTypeLicense";
            this.cmbTypeLicense.Size = new System.Drawing.Size(262, 20);
            this.cmbTypeLicense.TabIndex = 2;
            this.cmbTypeLicense.Items.Add("Permanente");
            this.cmbTypeLicense.Items.Add("Temporal");
            this.cmbTypeLicense.Tag = new string[] { "P", "T" };

            this.cmbTypeLicense.SelectedIndexChanged += (sender, e) =>
            {

                string[] internalValues = (string[])this.cmbTypeLicense.Tag;


                if (this.cmbTypeLicense.SelectedIndex >= 0 && this.cmbTypeLicense.SelectedIndex < internalValues.Length)
                {

                    string selectedInternalValue = internalValues[this.cmbTypeLicense.SelectedIndex];


                    Console.WriteLine("Valor interno seleccionado: " + selectedInternalValue);
                    this.FinalLicenseType = selectedInternalValue;
                }
            };
            // 
            // lbTypeLicense
            // 
            this.lbTypeLicense.AutoSize = true;
            this.lbTypeLicense.Location = new System.Drawing.Point(5, 104);
            this.lbTypeLicense.Name = "lbTypeLicense";
            this.lbTypeLicense.Size = new System.Drawing.Size(71, 13);
            this.lbTypeLicense.TabIndex = 2;
            this.lbTypeLicense.Text = "TipoLicencia:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(102, 130);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(262, 20);
            this.txtKey.TabIndex = 2;
            // 
            // lbKey
            // 
            this.lbKey.AutoSize = true;
            this.lbKey.Location = new System.Drawing.Point(5, 134);
            this.lbKey.Name = "lbKey";
            this.lbKey.Size = new System.Drawing.Size(28, 13);
            this.lbKey.TabIndex = 2;
            this.lbKey.Text = "Key:";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(55, 190);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(262, 20);
            this.txtResult.TabIndex = 2;
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.Visible = false;
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(5, 170);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(28, 13);
            this.lbResult.TabIndex = 2;
            this.lbResult.Text = "Resultado:";
            this.lbResult.Visible = false;
            // 
            // txtDisFirm
            // 
            this.txtDisFirm.Location = new System.Drawing.Point(102, 10);
            this.txtDisFirm.Name = "txtDisFirm";
            this.txtDisFirm.Size = new System.Drawing.Size(262, 20);
            this.txtDisFirm.TabIndex = 2;
            this.txtDisFirm.Visible = false;
            // 
            // lbDisFirm
            // 
            this.lbDisFirm.AutoSize = true;
            this.lbDisFirm.Location = new System.Drawing.Point(5, 14);
            this.lbDisFirm.Name = "lbDisFirm";
            this.lbDisFirm.Size = new System.Drawing.Size(35, 13);
            this.lbDisFirm.TabIndex = 2;
            this.lbDisFirm.Text = "Firma:";
            this.lbDisFirm.Visible = false;
            // 
            // txtDisKey
            // 
            this.txtDisKey.Location = new System.Drawing.Point(102, 40);
            this.txtDisKey.Name = "txtDisKey";
            this.txtDisKey.Size = new System.Drawing.Size(262, 20);
            this.txtDisKey.TabIndex = 2;
            this.txtDisKey.Visible = false;
            // 
            // lbDisKey
            // 
            this.lbDisKey.AutoSize = true;
            this.lbDisKey.Location = new System.Drawing.Point(5, 44);
            this.lbDisKey.Name = "lbDisKey";
            this.lbDisKey.Size = new System.Drawing.Size(28, 13);
            this.lbDisKey.TabIndex = 2;
            this.lbDisKey.Text = "Key:";
            this.lbDisKey.Visible = false;
            // 
            // txtDisResult
            // 
            this.txtDisResult.Location = new System.Drawing.Point(55, 100);
            this.txtDisResult.Name = "txtDisResult";
            this.txtDisResult.Size = new System.Drawing.Size(262, 20);
            this.txtDisResult.TabIndex = 2;
            this.txtDisResult.BorderStyle = BorderStyle.None;
            this.txtDisResult.Visible = false;
            // 
            // lbDisResult
            // 
            this.lbDisResult.AutoSize = true;
            this.lbDisResult.Location = new System.Drawing.Point(5, 80);
            this.lbDisResult.Name = "lbDisResult";
            this.lbDisResult.Size = new System.Drawing.Size(28, 13);
            this.lbDisResult.TabIndex = 2;
            this.lbDisResult.Text = "Resultado:";
            this.lbDisResult.Visible = false;
            // 
            // txtXMLFirm
            // 
            this.txtXMLFirm.Location = new System.Drawing.Point(102, 10);
            this.txtXMLFirm.Name = "txtXMLFirm";
            this.txtXMLFirm.ReadOnly = true;
            this.txtXMLFirm.Size = new System.Drawing.Size(262, 20);
            this.txtXMLFirm.TabIndex = 2;
            this.txtXMLFirm.Visible = false;
            // 
            // lbXMLFirm
            // 
            this.lbXMLFirm.AutoSize = true;
            this.lbXMLFirm.Location = new System.Drawing.Point(5, 14);
            this.lbXMLFirm.Name = "lbXMLFirm";
            this.lbXMLFirm.Size = new System.Drawing.Size(35, 13);
            this.lbXMLFirm.TabIndex = 2;
            this.lbXMLFirm.Text = "Firma:";
            this.lbXMLFirm.Visible = false;
            // 
            // txtXMLID
            // 
            this.txtXMLID.Location = new System.Drawing.Point(102, 70);
            this.txtXMLID.Name = "txtXMLID";
            this.txtXMLID.Size = new System.Drawing.Size(262, 20);
            this.txtXMLID.TabIndex = 2;
            this.txtXMLID.Visible = false;
            // 
            // lbXMLID
            // 
            this.lbXMLID.AutoSize = true;
            this.lbXMLID.Location = new System.Drawing.Point(5, 74);
            this.lbXMLID.Name = "lbXMLID";
            this.lbServerID.Size = new System.Drawing.Size(60, 13);
            this.lbXMLID.TabIndex = 2;
            this.lbXMLID.Text = "ID:";
            this.lbXMLID.Visible = false;
            //
            // richTextSentenceSQL
            //
            this.richTextSentenceSQL.Location = new System.Drawing.Point(5, 15);
            this.richTextSentenceSQL.Name = "richTextSentenceSQL";
            this.richTextSentenceSQL.Size = new System.Drawing.Size(370, 200);
            this.richTextSentenceSQL.TabIndex = 2;
            this.richTextSentenceSQL.BorderStyle = BorderStyle.None;
            this.richTextSentenceSQL.Visible = false;


            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 250);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolStrip);
            this.Icon = global::SignatureCreator.Properties.Resources.Logo;
            this.Name = "MainView";
            this.Text = "SignatureCreator";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton btnEncrypter;
        private ToolStripSeparator SeparatorBtnDisEncrypter;
        private ToolStripButton btnDisEncrypter;
        private ToolStripSeparator SeparatorBtnXMLGenerator;
        private ToolStripButton btnXMLGenerator;
        private ToolStripSeparator SeparatorBtnSentenceSQL;
        private ToolStripButton btnSentenceSQL;
        private RichTextBox richTextSentenceSQL;
        private Panel panel;
        private ComboBox cmbState;
        private Label lbState;
        private DateTimePicker timeDateLine;
        private Label lbDateline;
        private TextBox txtServerID;
        private Label lbServerID;
        private ComboBox cmbTypeLicense;
        private Label lbTypeLicense;
        private TextBox txtKey;
        private Label lbKey;
        private TextBox txtResult;
        private Label lbResult;
        private TextBox txtDisFirm;
        private Label lbDisFirm;
        private TextBox txtDisKey;
        private Label lbDisKey;
        private TextBox txtDisResult;
        private Label lbDisResult;
        private TextBox txtXMLFirm;
        private Label lbXMLFirm;
        private TextBox txtXMLID;
        private Label lbXMLID;

    }
}

