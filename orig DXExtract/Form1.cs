// Decompiled with JetBrains decompiler
// Type: DXExtract.Form1
// Assembly: DXExtract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 593F5E8D-8016-45A8-94A1-EA2F500EB23F
// Assembly location: U:\Unpackers\DXExtract\DXExtract.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DXExtract
{
  public class Form1 : Form
  {
    private string archivePath = "";
    private string[] supportedImages = new string[4]
    {
      ".png",
      ".jpg",
      ".tga",
      ".bmp"
    };
    private DXParser parser;
    private List<Entry> entries;
    private IContainer components;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem1;
    private SplitContainer splitContainer1;
    private TreeView explorerView;
    private SplitContainer splitContainer2;
    private TextBox decryptKeyBox;
    private Label label1;
    private OpenFileDialog openFileDialog1;
    private TextBox pathBox;
    private Label label2;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel statusLabel;
    private SaveFileDialog saveFileDialog1;
    private PictureBox picPreview;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem testToolStripMenuItem;
    private ToolStripMenuItem exportToolStripMenuItem1;
    private ToolStripMenuItem saveAsToolStripMenuItem1;

    public Form1()
    {
      this.InitializeComponent();
    }

    private void Form1_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;
      e.Effect = DragDropEffects.Copy;
    }

    private void Form1_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
        e.Effect = DragDropEffects.Copy;
      this.openFile(((string[]) e.Data.GetData(DataFormats.FileDrop))[0]);
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.openFile(this.openFileDialog1.FileName);
    }

    private void processFile(string filename)
    {
      this.statusLabel.Text = "Parsing...";
      this.parser = new DXParser(filename);
      string str = this.parser.checkForKey();
      if (!(str != ""))
        return;
      this.decryptKeyBox.Text = "0x" + str;
      this.entries = this.parser.parseFile();
      if (this.entries != null)
        this.buildFileList(this.entries);
      this.statusLabel.Text = "Ready";
    }

    private void openFile(string filename)
    {
      this.explorerView.Nodes.Clear();
      this.archivePath = filename;
      this.pathBox.Text = filename;
      this.processFile(filename);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void buildFileList(List<Entry> entries)
    {
      for (int index = 0; index < entries.Count; ++index)
      {
        Entry entry = entries[index];
        string[] pathbits = entry.name.Split(Path.DirectorySeparatorChar);
        this.addPath(this.getRoot(pathbits[0]), pathbits, entry);
      }
    }

    private void addPath(TreeNode node, string[] pathbits, Entry e)
    {
      for (int index = 1; index < pathbits.Length; ++index)
        node = this.add_node(node, pathbits[index]);
      node.Tag = (object) e;
    }

    private TreeNode getRoot(string key)
    {
      if (this.explorerView.Nodes.ContainsKey(key))
        return this.explorerView.Nodes[key];
      return this.explorerView.Nodes.Add(key, key);
    }

    private TreeNode add_node(TreeNode node, string key)
    {
      if (node.Nodes.ContainsKey(key))
        return node.Nodes[key];
      return node.Nodes.Add(key, key);
    }

    private void showImage(Entry entry)
    {
      this.picPreview.Image = Image.FromStream((Stream) new MemoryStream(this.parser.getFiledata(entry)));
    }

    private bool isImage(Entry entry)
    {
      foreach (string supportedImage in this.supportedImages)
      {
        if (entry.name.EndsWith(supportedImage))
          return true;
      }
      return false;
    }

    private void determineAction(Entry entry)
    {
      if (!this.isImage(entry))
        return;
      this.showImage(entry);
    }

    private void explorerView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (this.explorerView.SelectedNode == null || this.explorerView.SelectedNode.Tag == null)
        return;
      this.determineAction((Entry) this.explorerView.SelectedNode.Tag);
    }

    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.parser.exportArchive();
    }

    private void exportNodes(TreeNode node)
    {
      if (node.Tag != null)
        this.parser.exportFile((Entry) node.Tag);
      foreach (TreeNode node1 in node.Nodes)
      {
        this.exportNodes(node1);
        if (node1.Tag != null)
          this.parser.exportFile((Entry) node1.Tag);
      }
    }

    private void testToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.explorerView.SelectedNode == null)
        return;
      this.exportNodes(this.explorerView.SelectedNode);
    }

    private void explorerView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      this.explorerView.SelectedNode = e.Node;
    }

    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.openFile(this.openFileDialog1.FileName);
    }

    private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.archivePath == "")
        return;
      this.parser.exportArchive();
    }

    private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.archivePath == "" || this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.parser.saveFile(this.saveFileDialog1.FileName);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.menuStrip1 = new MenuStrip();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.splitContainer1 = new SplitContainer();
      this.explorerView = new TreeView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.testToolStripMenuItem = new ToolStripMenuItem();
      this.splitContainer2 = new SplitContainer();
      this.label2 = new Label();
      this.pathBox = new TextBox();
      this.label1 = new Label();
      this.decryptKeyBox = new TextBox();
      this.picPreview = new PictureBox();
      this.openFileDialog1 = new OpenFileDialog();
      this.statusStrip1 = new StatusStrip();
      this.statusLabel = new ToolStripStatusLabel();
      this.saveFileDialog1 = new SaveFileDialog();
      this.exportToolStripMenuItem1 = new ToolStripMenuItem();
      this.saveAsToolStripMenuItem1 = new ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      ((ISupportInitialize) this.picPreview).BeginInit();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.exportToolStripMenuItem1,
        (ToolStripItem) this.saveAsToolStripMenuItem1
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(661, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(48, 20);
      this.toolStripMenuItem1.Text = "Open";
      this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
      this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 24);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.explorerView);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Size = new Size(661, 340);
      this.splitContainer1.SplitterDistance = 220;
      this.splitContainer1.TabIndex = 1;
      this.explorerView.ContextMenuStrip = this.contextMenuStrip1;
      this.explorerView.Dock = DockStyle.Fill;
      this.explorerView.Location = new Point(0, 0);
      this.explorerView.Name = "explorerView";
      this.explorerView.Size = new Size(216, 336);
      this.explorerView.TabIndex = 0;
      this.explorerView.AfterSelect += new TreeViewEventHandler(this.explorerView_AfterSelect);
      this.explorerView.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.explorerView_NodeMouseClick);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.testToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(108, 26);
      this.testToolStripMenuItem.Name = "testToolStripMenuItem";
      this.testToolStripMenuItem.Size = new Size(107, 22);
      this.testToolStripMenuItem.Text = "Export";
      this.testToolStripMenuItem.Click += new EventHandler(this.testToolStripMenuItem_Click);
      this.splitContainer2.BorderStyle = BorderStyle.Fixed3D;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.label2);
      this.splitContainer2.Panel1.Controls.Add((Control) this.pathBox);
      this.splitContainer2.Panel1.Controls.Add((Control) this.label1);
      this.splitContainer2.Panel1.Controls.Add((Control) this.decryptKeyBox);
      this.splitContainer2.Panel2.Controls.Add((Control) this.picPreview);
      this.splitContainer2.Size = new Size(437, 340);
      this.splitContainer2.SplitterDistance = 47;
      this.splitContainer2.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Archive Path";
      this.pathBox.Location = new Point(80, 4);
      this.pathBox.Name = "pathBox";
      this.pathBox.Size = new Size(273, 20);
      this.pathBox.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.ForeColor = SystemColors.ControlText;
      this.label1.Location = new Point(6, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Decrypt Key:";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.decryptKeyBox.Location = new Point(80, 25);
      this.decryptKeyBox.Name = "decryptKeyBox";
      this.decryptKeyBox.Size = new Size(273, 20);
      this.decryptKeyBox.TabIndex = 0;
      this.picPreview.Location = new Point(4, 4);
      this.picPreview.Name = "picPreview";
      this.picPreview.Size = new Size(190, 165);
      this.picPreview.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picPreview.TabIndex = 0;
      this.picPreview.TabStop = false;
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.FileOk += new CancelEventHandler(this.openFileDialog1_FileOk);
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.statusLabel
      });
      this.statusStrip1.Location = new Point(0, 364);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(661, 22);
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip1";
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new Size(39, 17);
      this.statusLabel.Text = "Ready";
      this.exportToolStripMenuItem1.Name = "exportToolStripMenuItem1";
      this.exportToolStripMenuItem1.Size = new Size(52, 20);
      this.exportToolStripMenuItem1.Text = "Export";
      this.exportToolStripMenuItem1.Click += new EventHandler(this.exportToolStripMenuItem1_Click);
      this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
      this.saveAsToolStripMenuItem1.Size = new Size(59, 20);
      this.saveAsToolStripMenuItem1.Text = "Save As";
      this.saveAsToolStripMenuItem1.Click += new EventHandler(this.saveAsToolStripMenuItem1_Click);
      this.AllowDrop = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(661, 386);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.menuStrip1);
      this.Controls.Add((Control) this.statusStrip1);
      this.Name = "Form1";
      this.Text = "DXExtract";
      this.DragDrop += new DragEventHandler(this.Form1_DragDrop);
      this.DragEnter += new DragEventHandler(this.Form1_DragEnter);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.picPreview).EndInit();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
