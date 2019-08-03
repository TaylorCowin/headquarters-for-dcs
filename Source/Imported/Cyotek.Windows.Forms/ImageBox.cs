﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cyotek.Windows.Forms
{
    // Cyotek ImageBox
    // Copyright (c) 2010 Cyotek. All Rights Reserved.
    // http://cyotek.com

    [DefaultProperty("Image"), ToolboxBitmap(typeof(ImageBox))]
    public partial class ImageBox : ScrollableControl
    {
        private static readonly int MinZoom = 10;
        private static readonly int MaxZoom = 3500;

        private bool _autoCenter;
        private bool _autoPan;
        private BorderStyle _borderStyle;
        private int _gridCellSize;
        private Color _gridColor;
        private Color _gridColorAlternate;
        [Category("Property Changed")]
        private ImageBoxGridDisplayMode _gridDisplayMode;
        private ImageBoxGridScale _gridScale;
        private Bitmap _gridTile;
        private System.Drawing.Image _image;
        private InterpolationMode _interpolationMode;
        private bool _invertMouse;
        private bool _isPanning;
        private bool _sizeToFit;
        private Point _startMousePosition;
        private Point _startScrollPosition;
        private TextureBrush _texture;
        private int _zoom;
        private int _zoomIncrement;
        private bool _zoomOnClick;

        public ImageBox()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.StandardDoubleClick, false);
            UpdateStyles();

            BackColor = Color.White;
            AutoSize = true;
            GridScale = ImageBoxGridScale.Small;
            GridDisplayMode = ImageBoxGridDisplayMode.Client;
            GridColor = Color.Gainsboro;
            GridColorAlternate = Color.White;
            GridCellSize = 8;
            BorderStyle = BorderStyle.FixedSingle;
            AutoPan = true;
            Zoom = 100;
            ZoomIncrement = 20;
            InterpolationMode = InterpolationMode.Default;
            AutoCenter = true;
        }

        [Category("Property Changed")]
        public event EventHandler AutoCenterChanged;

        [Category("Property Changed")]
        public event EventHandler AutoPanChanged;

        [Category("Property Changed")]
        public event EventHandler BorderStyleChanged;

        [Category("Property Changed")]
        public event EventHandler GridCellSizeChanged;

        [Category("Property Changed")]
        public event EventHandler GridColorAlternateChanged;

        [Category("Property Changed")]
        public event EventHandler GridColorChanged;

        [Category("Property Changed")]
        public event EventHandler GridDisplayModeChanged;

        [Category("Property Changed")]
        public event EventHandler GridScaleChanged;

        [Category("Property Changed")]
        public event EventHandler ImageChanged;

        [Category("Property Changed")]
        public event EventHandler InterpolationModeChanged;

        [Category("Property Changed")]
        public event EventHandler InvertMouseChanged;

        [Category("Property Changed")]
        public event EventHandler PanEnd;

        [Category("Property Changed")]
        public event EventHandler PanStart;

        [Category("Property Changed")]
        public event EventHandler SizeToFitChanged;

        [Category("Property Changed")]
        public event EventHandler ZoomChanged;

        [Category("Property Changed")]
        public event EventHandler ZoomIncrementChanged;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                if (base.AutoSize != value)
                {
                    base.AutoSize = value;
                    AdjustLayout();
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size size;

            if (Image != null)
            {
                int width;
                int height;

                // get the size of the image
                width = ScaledImageWidth;
                height = ScaledImageHeight;

                // add an offset based on padding
                width += Padding.Horizontal;
                height += Padding.Vertical;

                // add an offset based on the border style
                width += GetBorderOffset();
                height += GetBorderOffset();

                size = new Size(width, height);
            }
            else
                size = base.GetPreferredSize(proposedSize);

            return size;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (_texture != null)
                {
                    _texture.Dispose();
                    _texture = null;
                }

                if (_gridTile != null)
                {
                    _gridTile.Dispose();
                    _gridTile = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            bool result;

            if ((keyData & Keys.Right) == Keys.Right | (keyData & Keys.Left) == Keys.Left | (keyData & Keys.Up) == Keys.Up | (keyData & Keys.Down) == Keys.Down)
                result = true;
            else
                result = base.IsInputKey(keyData);

            return result;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            Invalidate();
        }

        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);

            if (Dock != DockStyle.None)
                AutoSize = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    AdjustScroll(-(e.Modifiers == Keys.None ? HorizontalScroll.SmallChange : HorizontalScroll.LargeChange), 0);
                    break;
                case Keys.Right:
                    AdjustScroll(e.Modifiers == Keys.None ? HorizontalScroll.SmallChange : HorizontalScroll.LargeChange, 0);
                    break;
                case Keys.Up:
                    AdjustScroll(0, -(e.Modifiers == Keys.None ? VerticalScroll.SmallChange : VerticalScroll.LargeChange));
                    break;
                case Keys.Down:
                    AdjustScroll(0, e.Modifiers == Keys.None ? VerticalScroll.SmallChange : VerticalScroll.LargeChange);
                    break;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (ZoomOnClick && !IsPanning && !SizeToFit)
            {
                if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                {
                    if (Zoom >= 100)
                        Zoom = (int)Math.Round((double)(Zoom + 100) / 100) * 100;
                    else if (Zoom >= 75)
                        Zoom = 100;
                    else
                        Zoom = (int)(Zoom / 0.75F);
                }
                else if (e.Button == MouseButtons.Right || (e.Button == MouseButtons.Left && Control.ModifierKeys != Keys.None))
                {
                    if (Zoom > 100 && Zoom <= 125)
                        Zoom = 100;
                    else if (Zoom > 100)
                        Zoom = (int)Math.Round((double)(Zoom - 100) / 100) * 100;
                    else
                        Zoom = (int)(Zoom * 0.75F);
                }
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Focused)
                Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left && AutoPan && Image != null)
            {
                if (!IsPanning)
                {
                    _startMousePosition = e.Location;
                    IsPanning = true;
                }

                if (IsPanning)
                {
                    int x;
                    int y;
                    Point position;

                    if (!InvertMouse)
                    {
                        x = -_startScrollPosition.X + (_startMousePosition.X - e.Location.X);
                        y = -_startScrollPosition.Y + (_startMousePosition.Y - e.Location.Y);
                    }
                    else
                    {
                        x = -(_startScrollPosition.X + (_startMousePosition.X - e.Location.X));
                        y = -(_startScrollPosition.Y + (_startMousePosition.Y - e.Location.Y));
                    }

                    position = new Point(x, y);

                    UpdateScrollPosition(position);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsPanning)
                IsPanning = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!SizeToFit)
            {
                int increment;

                if (Control.ModifierKeys == Keys.None)
                    increment = ZoomIncrement;
                else
                    increment = ZoomIncrement * 5;

                if (e.Delta < 0)
                    increment = -increment;

                Zoom += increment;
            }
        }

        protected override void OnPaddingChanged(System.EventArgs e)
        {
            base.OnPaddingChanged(e);
            AdjustLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle innerRectangle;

            // draw the borders
            switch (BorderStyle)
            {
                case BorderStyle.FixedSingle:
                    ControlPaint.DrawBorder(e.Graphics, ClientRectangle, ForeColor, ButtonBorderStyle.Solid);
                    break;
                case BorderStyle.Fixed3D:
                    ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
                    break;
            }

            innerRectangle = GetInsideViewPort();

            // draw the background
            using (SolidBrush brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, innerRectangle);

            if (_texture != null && GridDisplayMode != ImageBoxGridDisplayMode.None)
            {
                switch (GridDisplayMode)
                {
                    case ImageBoxGridDisplayMode.Image:
                        Rectangle fillRectangle;

                        fillRectangle = GetImageViewPort();
                        e.Graphics.FillRectangle(_texture, fillRectangle);

                        if (!fillRectangle.Equals(innerRectangle))
                        {
                            fillRectangle.Inflate(1, 1);
                            ControlPaint.DrawBorder(e.Graphics, fillRectangle, ForeColor, ButtonBorderStyle.Solid);
                        }
                        break;
                    case ImageBoxGridDisplayMode.Client:
                        e.Graphics.FillRectangle(_texture, innerRectangle);
                        break;
                }
            }

            // draw the image
            if (Image != null)
                DrawImage(e.Graphics);

            base.OnPaint(e);
        }

        protected override void OnParentChanged(System.EventArgs e)
        {
            base.OnParentChanged(e);
            AdjustLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            AdjustLayout();

            base.OnResize(e);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            Invalidate();

            base.OnScroll(se);
        }

        public virtual Rectangle GetImageViewPort()
        {
            Rectangle viewPort;

            if (Image != null)
            {
                Rectangle innerRectangle;
                Point offset;

                innerRectangle = GetInsideViewPort();

                if (AutoCenter)
                {
                    int x;
                    int y;

                    x = !HScroll ? (innerRectangle.Width - (ScaledImageWidth + Padding.Horizontal)) / 2 : 0;
                    y = !VScroll ? (innerRectangle.Height - (ScaledImageHeight + Padding.Vertical)) / 2 : 0;

                    offset = new Point(x, y);
                }
                else
                    offset = Point.Empty;

                viewPort = new Rectangle(offset.X + innerRectangle.Left + Padding.Left, offset.Y + innerRectangle.Top + Padding.Top, innerRectangle.Width - (Padding.Horizontal + (offset.X * 2)), innerRectangle.Height - (Padding.Vertical + (offset.Y * 2)));
            }
            else
                viewPort = Rectangle.Empty;

            return viewPort;
        }

        public Rectangle GetInsideViewPort()
        {
            return GetInsideViewPort(false);
        }

        public virtual Rectangle GetInsideViewPort(bool includePadding)
        {
            int left;
            int top;
            int width;
            int height;
            int borderOffset;

            borderOffset = GetBorderOffset();
            left = borderOffset;
            top = borderOffset;
            width = ClientSize.Width - (borderOffset * 2);
            height = ClientSize.Height - (borderOffset * 2);

            if (includePadding)
            {
                left += Padding.Left;
                top += Padding.Top;
                width -= Padding.Horizontal;
                height -= Padding.Vertical;
            }

            return new Rectangle(left, top, width, height);
        }

        public virtual Rectangle GetSourceImageRegion()
        {
            int sourceLeft;
            int sourceTop;
            int sourceWidth;
            int sourceHeight;
            Rectangle viewPort;
            Rectangle region;

            if (Image != null)
            {
                viewPort = GetImageViewPort();
                sourceLeft = (int)(-AutoScrollPosition.X / ZoomFactor);
                sourceTop = (int)(-AutoScrollPosition.Y / ZoomFactor);
                sourceWidth = (int)(viewPort.Width / ZoomFactor);
                sourceHeight = (int)(viewPort.Height / ZoomFactor);

                region = new Rectangle(sourceLeft, sourceTop, sourceWidth, sourceHeight);
            }
            else
                region = Rectangle.Empty;

            return region;
        }

        public virtual void ZoomToFit()
        {
            if (Image != null)
            {
                Rectangle innerRectangle;
                double zoom;
                double aspectRatio;

                AutoScrollMinSize = Size.Empty;

                innerRectangle = GetInsideViewPort(true);

                if (Image.Width > Image.Height)
                {
                    aspectRatio = ((double)innerRectangle.Width) / ((double)Image.Width);
                    zoom = aspectRatio * 100.0;

                    if (innerRectangle.Height < ((Image.Height * zoom) / 100.0))
                    {
                        aspectRatio = ((double)innerRectangle.Height) / ((double)Image.Height);
                        zoom = aspectRatio * 100.0;
                    }
                }
                else
                {
                    aspectRatio = ((double)innerRectangle.Height) / ((double)Image.Height);
                    zoom = aspectRatio * 100.0;

                    if (innerRectangle.Width < ((Image.Width * zoom) / 100.0))
                    {
                        aspectRatio = ((double)innerRectangle.Width) / ((double)Image.Width);
                        zoom = aspectRatio * 100.0;
                    }
                }

                Zoom = (int)Math.Round(Math.Floor(zoom));
            }
        }

        [DefaultValue(true), Category("Appearance")]
        public bool AutoCenter
        {
            get { return _autoCenter; }
            set
            {
                if (_autoCenter != value)
                {
                    _autoCenter = value;
                    OnAutoCenterChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(true), Category("Behavior")]
        public bool AutoPan
        {
            get { return _autoPan; }
            set
            {
                if (_autoPan != value)
                {
                    _autoPan = value;
                    OnAutoPanChanged(EventArgs.Empty);

                    if (value)
                        SizeToFit = false;
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }

        [Category("Appearance"), DefaultValue(typeof(BorderStyle), "FixedSingle")]
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (_borderStyle != value)
                {
                    _borderStyle = value;
                    OnBorderStyleChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance"), DefaultValue(8)]
        public int GridCellSize
        {
            get { return _gridCellSize; }
            set
            {
                if (_gridCellSize != value)
                {
                    _gridCellSize = value;
                    OnGridCellSizeChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "Gainsboro")]
        public Color GridColor
        {
            get { return _gridColor; }
            set
            {
                if (_gridColor != value)
                {
                    _gridColor = value;
                    OnGridColorChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "White")]
        public Color GridColorAlternate
        {
            get { return _gridColorAlternate; }
            set
            {
                if (_gridColorAlternate != value)
                {
                    _gridColorAlternate = value;
                    OnGridColorAlternateChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(ImageBoxGridDisplayMode.Client), Category("Appearance")]
        public ImageBoxGridDisplayMode GridDisplayMode
        {
            get { return _gridDisplayMode; }
            set
            {
                if (_gridDisplayMode != value)
                {
                    _gridDisplayMode = value;
                    OnGridDisplayModeChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(ImageBoxGridScale), "Small"), Category("Appearance")]
        public ImageBoxGridScale GridScale
        {
            get { return _gridScale; }
            set
            {
                if (_gridScale != value)
                {
                    _gridScale = value;
                    OnGridScaleChanged(EventArgs.Empty);
                }
            }
        }

        [Category("Appearance"), DefaultValue(null)]
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnImageChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(InterpolationMode.Default), Category("Appearance")]
        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    value = InterpolationMode.Default;

                if (_interpolationMode != value)
                {
                    _interpolationMode = value;
                    OnInterpolationModeChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(false), Category("Behavior")]
        public bool InvertMouse
        {
            get { return _invertMouse; }
            set
            {
                if (_invertMouse != value)
                {
                    _invertMouse = value;
                    OnInvertMouseChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool IsPanning
        {
            get { return _isPanning; }
            protected set
            {
                if (_isPanning != value)
                {
                    _isPanning = value;
                    _startScrollPosition = AutoScrollPosition;

                    if (value)
                    {
                        Cursor = Cursors.SizeAll;
                        OnPanStart(EventArgs.Empty);
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        OnPanEnd(EventArgs.Empty);
                    }
                }
            }
        }

        [DefaultValue(false), Category("Appearance")]
        public bool SizeToFit
        {
            get { return _sizeToFit; }
            set
            {
                if (_sizeToFit != value)
                {
                    _sizeToFit = value;
                    OnSizeToFitChanged(EventArgs.Empty);

                    if (value)
                        AutoPan = false;
                }
            }
        }

        [DefaultValue(100), Category("Appearance")]
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                if (value < ImageBox.MinZoom)
                    value = ImageBox.MinZoom;
                else if (value > ImageBox.MaxZoom)
                    value = ImageBox.MaxZoom;

                if (_zoom != value)
                {
                    _zoom = value;
                    OnZoomChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(20), Category("Behavior")]
        public int ZoomIncrement
        {
            get { return _zoomIncrement; }
            set
            {
                if (_zoomIncrement != value)
                {
                    _zoomIncrement = value;
                    OnZoomIncrementChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(true), Category("Behavior")]
        public bool ZoomOnClick
        {
            get { return _zoomOnClick; }
            set { _zoomOnClick = value; }
        }

        private int GetBorderOffset()
        {
            int offset;

            switch (BorderStyle)
            {
                case BorderStyle.Fixed3D:
                    offset = 2;
                    break;
                case BorderStyle.FixedSingle:
                    offset = 1;
                    break;
                default:
                    offset = 0;
                    break;
            }

            return offset;
        }

        private void InitializeGridTile()
        {
            if (_texture != null)
                _texture.Dispose();

            if (_gridTile != null)
                _gridTile.Dispose();

            if (GridDisplayMode != ImageBoxGridDisplayMode.None && GridCellSize != 0)
            {
                _gridTile = CreateGridTileImage(GridCellSize, GridColor, GridColorAlternate);
                _texture = new TextureBrush(_gridTile);
            }

            Invalidate();
        }

        protected virtual int ScaledImageHeight
        { get { return Image != null ? (int)(Image.Size.Height * ZoomFactor) : 0; } }

        protected virtual int ScaledImageWidth
        { get { return Image != null ? (int)(Image.Size.Width * ZoomFactor) : 0; } }

        protected virtual double ZoomFactor
        { get { return (double)Zoom / 100; } }

        protected virtual void AdjustLayout()
        {
            if (AutoSize)
                AdjustSize();
            else if (SizeToFit)
                ZoomToFit();
            else if (AutoScroll)
                AdjustViewPort();
            Invalidate();
        }

        protected virtual void AdjustScroll(int x, int y)
        {
            Point scrollPosition;

            scrollPosition = new Point(HorizontalScroll.Value + x, VerticalScroll.Value + y);

            UpdateScrollPosition(scrollPosition);
        }

        protected virtual void AdjustSize()
        {
            if (AutoSize && Dock == DockStyle.None)
                base.Size = base.PreferredSize;
        }

        protected virtual void AdjustViewPort()
        {
            if (AutoScroll && Image != null)
                AutoScrollMinSize = new Size(ScaledImageWidth + Padding.Horizontal, ScaledImageHeight + Padding.Vertical);
        }

        protected virtual Bitmap CreateGridTileImage(int cellSize, Color firstColor, Color secondColor)
        {
            Bitmap result;
            int width;
            int height;
            float scale;

            // rescale the cell size
            switch (GridScale)
            {
                case ImageBoxGridScale.Medium:
                    scale = 1.5F;
                    break;
                case ImageBoxGridScale.Large:
                    scale = 2;
                    break;
                default:
                    scale = 1;
                    break;
            }

            cellSize = (int)(cellSize * scale);

            // draw the tile
            width = cellSize * 2;
            height = cellSize * 2;
            result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                using (SolidBrush brush = new SolidBrush(firstColor))
                    g.FillRectangle(brush, new Rectangle(0, 0, width, height));

                using (SolidBrush brush = new SolidBrush(secondColor))
                {
                    g.FillRectangle(brush, new Rectangle(0, 0, cellSize, cellSize));
                    g.FillRectangle(brush, new Rectangle(cellSize, cellSize, cellSize, cellSize));
                }
            }

            return result;
        }

        protected virtual void DrawImage(Graphics g)
        {
            g.InterpolationMode = InterpolationMode;
            g.DrawImage(Image, GetImageViewPort(), GetSourceImageRegion(), GraphicsUnit.Pixel);
        }

        protected virtual void OnAutoCenterChanged(EventArgs e)
        {
            Invalidate();

            AutoCenterChanged?.Invoke(this, e);
        }

        protected virtual void OnAutoPanChanged(EventArgs e)
        {
            AutoPanChanged?.Invoke(this, e);
        }

        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            AdjustLayout();

            BorderStyleChanged?.Invoke(this, e);
        }

        protected virtual void OnGridCellSizeChanged(EventArgs e)
        {
            InitializeGridTile();

            GridCellSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnGridColorAlternateChanged(EventArgs e)
        {
            InitializeGridTile();

            GridColorAlternateChanged?.Invoke(this, e);
        }

        protected virtual void OnGridColorChanged(EventArgs e)
        {
            InitializeGridTile();

            GridColorChanged?.Invoke(this, e);

        }

        protected virtual void OnGridDisplayModeChanged(EventArgs e)
        {
            InitializeGridTile();
            Invalidate();

            GridDisplayModeChanged?.Invoke(this, e);
        }

        protected virtual void OnGridScaleChanged(EventArgs e)
        {
            InitializeGridTile();

            GridScaleChanged?.Invoke(this, e);
        }

        protected virtual void OnImageChanged(EventArgs e)
        {
            AdjustLayout();

            ImageChanged?.Invoke(this, e);
        }

        protected virtual void OnInterpolationModeChanged(EventArgs e)
        {
            Invalidate();

            InterpolationModeChanged?.Invoke(this, e);
        }

        protected virtual void OnInvertMouseChanged(EventArgs e)
        {
            InvertMouseChanged?.Invoke(this, e);
        }

        protected virtual void OnPanEnd(EventArgs e)
        {
            PanEnd?.Invoke(this, e);
        }

        protected virtual void OnPanStart(EventArgs e)
        {
            PanStart?.Invoke(this, e);
        }

        protected virtual void OnSizeToFitChanged(EventArgs e)
        {
            AdjustLayout();

            SizeToFitChanged?.Invoke(this, e);
        }

        protected virtual void OnZoomChanged(EventArgs e)
        {
            AdjustLayout();

            ZoomChanged?.Invoke(this, e);
        }

        protected virtual void OnZoomIncrementChanged(EventArgs e)
        {
            ZoomIncrementChanged?.Invoke(this, e);
        }

        protected virtual void UpdateScrollPosition(Point position)
        {
            AutoScrollPosition = position;
            Invalidate();
            OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, 0));
        }
    }
}
