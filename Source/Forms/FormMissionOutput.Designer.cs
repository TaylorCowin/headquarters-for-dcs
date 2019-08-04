﻿namespace Headquarters4DCS.Forms
{
    partial class FormMissionOutput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMissionOutput));
            this.MissionOutputToolStrip = new System.Windows.Forms.ToolStrip();
            this.GenerateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ExportMizToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ExportBriefingToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ExportBriefingToHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BriefingLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BriefingWebBrowser = new System.Windows.Forms.WebBrowser();
            this.ExportBriefingToJPEGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportBriefingToPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MissionOutputToolStrip.SuspendLayout();
            this.BriefingLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MissionOutputToolStrip
            // 
            this.MissionOutputToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GenerateToolStripButton,
            this.ExportMizToolStripButton,
            this.ExportBriefingToolStripDropDownButton});
            this.MissionOutputToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MissionOutputToolStrip.Name = "MissionOutputToolStrip";
            this.MissionOutputToolStrip.Size = new System.Drawing.Size(624, 25);
            this.MissionOutputToolStrip.TabIndex = 0;
            this.MissionOutputToolStrip.Text = "toolStrip1";
            // 
            // GenerateToolStripButton
            // 
            this.GenerateToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("GenerateToolStripButton.Image")));
            this.GenerateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GenerateToolStripButton.Name = "GenerateToolStripButton";
            this.GenerateToolStripButton.Size = new System.Drawing.Size(162, 22);
            this.GenerateToolStripButton.Text = "Generate another mission";
            this.GenerateToolStripButton.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // ExportMizToolStripButton
            // 
            this.ExportMizToolStripButton.Enabled = false;
            this.ExportMizToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportMizToolStripButton.Image")));
            this.ExportMizToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportMizToolStripButton.Name = "ExportMizToolStripButton";
            this.ExportMizToolStripButton.Size = new System.Drawing.Size(143, 22);
            this.ExportMizToolStripButton.Text = "Export mission to .miz";
            this.ExportMizToolStripButton.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // ExportBriefingToolStripDropDownButton
            // 
            this.ExportBriefingToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportBriefingToHTMLToolStripMenuItem,
            this.ExportBriefingToJPEGToolStripMenuItem,
            this.ExportBriefingToPNGToolStripMenuItem});
            this.ExportBriefingToolStripDropDownButton.Enabled = false;
            this.ExportBriefingToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportBriefingToolStripDropDownButton.Image")));
            this.ExportBriefingToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportBriefingToolStripDropDownButton.Name = "ExportBriefingToolStripDropDownButton";
            this.ExportBriefingToolStripDropDownButton.Size = new System.Drawing.Size(136, 22);
            this.ExportBriefingToolStripDropDownButton.Text = "Export briefing to...";
            // 
            // ExportBriefingToHTMLToolStripMenuItem
            // 
            this.ExportBriefingToHTMLToolStripMenuItem.Name = "ExportBriefingToHTMLToolStripMenuItem";
            this.ExportBriefingToHTMLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExportBriefingToHTMLToolStripMenuItem.Text = "HTML";
            this.ExportBriefingToHTMLToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // BriefingLayoutPanel
            // 
            this.BriefingLayoutPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BriefingLayoutPanel.ColumnCount = 3;
            this.BriefingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BriefingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 512F));
            this.BriefingLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BriefingLayoutPanel.Controls.Add(this.BriefingWebBrowser, 1, 0);
            this.BriefingLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BriefingLayoutPanel.Location = new System.Drawing.Point(0, 25);
            this.BriefingLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BriefingLayoutPanel.Name = "BriefingLayoutPanel";
            this.BriefingLayoutPanel.RowCount = 1;
            this.BriefingLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.BriefingLayoutPanel.Size = new System.Drawing.Size(624, 416);
            this.BriefingLayoutPanel.TabIndex = 1;
            // 
            // BriefingWebBrowser
            // 
            this.BriefingWebBrowser.AllowNavigation = false;
            this.BriefingWebBrowser.AllowWebBrowserDrop = false;
            this.BriefingWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BriefingWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.BriefingWebBrowser.Location = new System.Drawing.Point(59, 3);
            this.BriefingWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.BriefingWebBrowser.Name = "BriefingWebBrowser";
            this.BriefingWebBrowser.Size = new System.Drawing.Size(506, 410);
            this.BriefingWebBrowser.TabIndex = 1;
            this.BriefingWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // ExportBriefingToJPEGToolStripMenuItem
            // 
            this.ExportBriefingToJPEGToolStripMenuItem.Name = "ExportBriefingToJPEGToolStripMenuItem";
            this.ExportBriefingToJPEGToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExportBriefingToJPEGToolStripMenuItem.Text = "JPEG";
            this.ExportBriefingToJPEGToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // ExportBriefingToPNGToolStripMenuItem
            // 
            this.ExportBriefingToPNGToolStripMenuItem.Name = "ExportBriefingToPNGToolStripMenuItem";
            this.ExportBriefingToPNGToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExportBriefingToPNGToolStripMenuItem.Text = "PNG";
            this.ExportBriefingToPNGToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // FormMissionOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.BriefingLayoutPanel);
            this.Controls.Add(this.MissionOutputToolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(512, 384);
            this.Name = "FormMissionOutput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMissionOutput";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMissionOutput_FormClosed);
            this.Load += new System.EventHandler(this.FormMissionOutput_Load);
            this.MissionOutputToolStrip.ResumeLayout(false);
            this.MissionOutputToolStrip.PerformLayout();
            this.BriefingLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MissionOutputToolStrip;
        private System.Windows.Forms.ToolStripButton GenerateToolStripButton;
        private System.Windows.Forms.TableLayoutPanel BriefingLayoutPanel;
        private System.Windows.Forms.WebBrowser BriefingWebBrowser;
        private System.Windows.Forms.ToolStripButton ExportMizToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton ExportBriefingToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem ExportBriefingToHTMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportBriefingToJPEGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportBriefingToPNGToolStripMenuItem;
    }
}