using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;

namespace OutlookAddIn
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            /*Outlook.Inspectors inspectors;
            
            inspectors = this.Application.Inspectors;
            inspectors.NewInspector +=
            new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);*/
            //SendEmailtoContacts();
            CreateEmailItemAndSend("TEST","francosalvucci14@gmail.com","TEST BODY");
        }
        /*
         * 
        private void Inspectors_NewInspector(Inspector Inspector)
        {
            Outlook.MailItem mailItem = Inspector.CurrentItem as Outlook.MailItem;
            if (mailItem != null)
            {
                if (mailItem.EntryID == null)
                {
                    mailItem.Subject = "This text was added by using code";
                    mailItem.Body = "This text was added by using code";
                }

            }
        }*/
        /*
         * 
        private void SendEmailtoContacts()
        {
            string subjectEmail = "Meeting has been rescheduled.";
            string bodyEmail = "Meeting is one hour later.";
            Outlook.MAPIFolder sentContacts = (Outlook.MAPIFolder)
                this.Application.ActiveExplorer().Session.GetDefaultFolder
                (Outlook.OlDefaultFolders.olFolderInbox);
            
            foreach (Outlook.ContactItem contact in sentContacts.Items)
            {
                if (contact.Email1Address.Contains("gmail.com"))
                {
                    this.CreateEmailItem(subjectEmail, contact
                        .Email1Address, bodyEmail);
                }
            }
        }

        private void CreateEmailItem(string subjectEmail,
                string toEmail, string bodyEmail)
         {
             Outlook.MailItem eMail = (Outlook.MailItem)
                 this.Application.CreateItem(Outlook.OlItemType.olMailItem);
             eMail.Subject = subjectEmail;
             eMail.To = toEmail;
             eMail.Body = bodyEmail;
             eMail.Importance = Outlook.OlImportance.olImportanceLow;
             ((Outlook._MailItem)eMail).Send();
         }
        */
        //FOR SEND EMAIL FROM OUTLOOK
        public void CreateEmailItemAndSend(string subjectEmail,
                string toEmail, string bodyEmail)
        {
            Outlook.MailItem eMail = (Outlook.MailItem)
                this.Application.CreateItem(Outlook.OlItemType.olMailItem);
            eMail.Subject = subjectEmail;
            eMail.To = toEmail;
            eMail.Body = bodyEmail;
            eMail.Importance = Outlook.OlImportance.olImportanceLow;
            ((Outlook._MailItem)eMail).Send();
        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Nota: Outlook non genera più questo evento. Se è presente codice che 
            //    deve essere eseguito all'arresto di Outlook, vedere https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region Codice generato da VSTO

        /// <summary>
        /// Metodo richiesto per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
