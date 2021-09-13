﻿using S50cBL22;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stdole;
using S50cSys22;
using S50cUtil22;
using Sage50c.API;

namespace Sage50c.ExtenderSample22 {
    public partial class FormCallMenu : Form, IChildPanel {

        private IChildWindow childWindow = null;
        private IUITransaction uiTransactionManager = null;
        private IUIMasterTable uiMasterTableManager = null;
        private IUIMenu uiMenuManager = null;

        public FormCallMenu() {
            InitializeComponent();
        }

        public void ShowWindow() {
            if (childWindow == null) {
                childWindow = SystemHandler.GeneralChildWindow.GetNewInstance();
            }

            childWindow.BorderStyle = VBRUN.FormBorderStyleConstants.vbSizable;
            childWindow.Caption = "Menu";
            childWindow.IDFieldCaption = "Artigo:";
            childWindow.SearchButtonVisible = false;
            childWindow.NavigationButtonsVisible = false;
            childWindow.IDFieldVisible = false;

            if (uiTransactionManager == null) {
                uiTransactionManager = SystemHandler.GeneralUITransaction;
            }

            if (uiMasterTableManager == null) {
                uiMasterTableManager = SystemHandler.GeneralUIMasterTable;
            }
                        
            if (uiMenuManager == null) {
                uiMenuManager = SystemHandler.GeneralUIMenu;
            }
            //
            // Construir menus da janela
            // 1. Botões
            var xternderMenuItems = new ExtenderMenuItems();
            var m = xternderMenuItems.Add("xAction1", "Acção 1");
            m.ActionType = ExtenderActionType.ExtenderActionPrimary;

            m = xternderMenuItems.Add("xAction2", "Acção 2");
            m.ActionType = ExtenderActionType.ExtenderActionSecondary;

            //
            //2. Opções
            m = xternderMenuItems.Add("xOpcoes", "Opções");
            m.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            m.BeginGroup = true;
            m.ChildItems.Add("xOpcoes1", "Opção 1");
            m.ChildItems.Add("xOpcoes2", "Opção 2");
            childWindow.MenuItems = xternderMenuItems;

            childWindow.Init(this);

            //Translate to twips
            childWindow.SetClientArea(this.Width * 15, this.Height * 15);
            childWindow.CenterOnScreen();
            

            this.Visible = true;
            childWindow.Show();

            //childWindow.WindowState = VBRUN.FormWindowStateConstants.vbMaximized;


            //Enable panel
            if (childWindow.EditState == EditStateType.esNull) {
                childWindow.EditState = EditStateType.esNew;
            }
        }

        public string Description {
            get {
                return "My Form Description";
            }
        }

        public int Handler {
            get {
                return this.Handle.ToInt32();
            }
        }

        public StdPicture Picture {
            get {
                return null;
            }
        }

        public string Title {
            get {
                return "My Title";
            }
        }

        public bool BeforeCancel() {
            MessageBox.Show("BeforeCancel");
            return true;
            //throw new NotImplementedException();
        }

        public bool BeforeOk() {
            MessageBox.Show("BeforeOK");
            return true;
            //throw new NotImplementedException();
        }

        public bool CheckIDValue(string value) {
            if (!string.IsNullOrEmpty(value)) {
                if (APIEngine.DSOCache.ItemProvider.ItemExist(value)) {
                    var item = APIEngine.DSOCache.ItemProvider.GetItem(value, APIEngine.SystemSettings.BaseCurrency);
                    MessageBox.Show(string.Format("ItemId={0}\r\nItem Name={1}", item.ItemID, item.Description));
                }
            }

            //MessageBox.Show("CheckIDValue: " + value );
            return true;
            //throw new NotImplementedException();
        }

        public bool OnMenuItem(string MenuItemID) {
            MessageBox.Show("MenuItemID: " + MenuItemID);
            return true;
            //throw new NotImplementedException();
        }

        public void SetBackcolor(int value) {
            this.BackColor = Color.FromArgb(value);
            //throw new NotImplementedException();
        }

        public void SetFocus() {
            //this.SetFocus();
        }

        public void SetSize(float Width, float Height) {
            SetBounds(0, 0, (int)(Width / 15), (int)(Height / 15));

            grpGroup.Left  = (int)((Width / 15 / 2) - grpGroup.Width /2);
            grpGroup.Top = (int)((Height / 15 / 2) - grpGroup.Height / 2);


        }

        private void FormManager_Load(object sender, EventArgs e) {
        }

        private void FormManager_FormClosed(object sender, FormClosedEventArgs e) {
            if (childWindow != null) {
                childWindow = null;
            }

            if (uiTransactionManager != null) {
                uiTransactionManager = null;
            }

            if (uiMasterTableManager != null) {
                uiMasterTableManager = null;
            }
            
            if (uiMenuManager != null) {
                uiMenuManager = null;
            }

        }

        private void btnShowTransaction_Click_1(object sender, EventArgs e) {
            uiTransactionManager.ShowTransaction(true, txtTransSerial.Text, txtTransDocument.Text, Convert.ToDouble(numTransdocnunber.Value));
        }

        private void btnShowItem_Click(object sender, EventArgs e) {
            uiMasterTableManager.ShowItem(txtItemId.Text, null, false);
        }


        private void btnMenu_Click(object sender, EventArgs e) {
            //See Name/Id in:
            //%ProgramFiles(x86)%\Sage 50c\UI\CRTL.PTG.UI.xml

            //<MenuItem Id="1010011" Name="miSaleTransaction" Title="Faturas"
            //UIMenuManager.ToolClick("miSaleTransaction", "1010011");

            //<MenuItem Id="20061" Name="miRelVTotDocEmitidos" Title="Lista de documentos de venda" 
            //UIMenuManager.ToolClick("miRelVTotDocEmitidos", null);

            //<MenuItem Id="40022" Name="miItemPriceChange" Title="Atualização automática assistida"
            //UIMenuManager.ToolClick("miItemPriceChange", null);

            //<MenuItem Id="10011" Name="miItem" Description="Artigos" Title="Artigos" 
            //UIMenuManager.ToolClick("miItem", null);

            uiMenuManager.ToolClick(txtMenu.Text, null);
        }


        /// <summary>
        /// Sample addons (modules)
        /// A: Gestão comercial
        /// LC: Loja Conenction
        /// PX: Pescados/Congelados
        /// H: Poweron
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckLicModule_Click(object sender, EventArgs e) {
            var modules = APIEngine.SystemSettings.License.License.ModulesList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                              .Select(x=>x.Trim());

            if( modules.Any(m=>string.Equals(txtLicModule.Text, m, StringComparison.CurrentCultureIgnoreCase)) ) {
                MessageBox.Show($"O módulo {txtLicModule.Text} existe na licença", 
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show($"O módulo {txtLicModule.Text} Não existe na licença",
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
