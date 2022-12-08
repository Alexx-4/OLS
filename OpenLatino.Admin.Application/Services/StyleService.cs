using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.MapperStyle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace OpenLatino.Admin.Application.Services
{
    public class StyleService : CRUD_Service<VectorStyle>, IStyleHelper
    {
        private IRepository<Layer> layerRepo;

        public StyleService(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            this.layerRepo = unitOfWork.Set<Layer>();
        }

        public IEnumerable<Layer> ListLayers()
        {
            return this.layerRepo.GetAll(l => true, true);
        }

        public CRUD_Service<VectorStyle> GetCRUD()
        {
            return this;
        }

        public byte[] getImage(VectorStyle vectorStyle)
        {
            var _style = MapperStyle.ToSysDrawingStyle(vectorStyle);


            int width = 308;
            int height = 80;

            Pen _blackPen = new Pen(Color.Black, 4);
            Brush _whiteBrush = new SolidBrush(Color.White);

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            g.FillRectangle(_whiteBrush, 0, 0, width, height);
            g.DrawRectangle(_blackPen, 0, 0, width, height);

        
            Rectangle _rect = new Rectangle(17, 10, 60, 60);
            if (_style.EnableOutLine)
            {
                _style.OutLineStyle.Width = 5;
                g.DrawRectangle(_style.OutLineStyle, _rect);
            }
            g.FillRectangle(_style.FillStyle, _rect);


            Point l1 = new Point(117, 70);
            Point l2 = new Point(187, 10);
            _style.LineStyle.Width = 8;
            g.DrawLine(_style.LineStyle, l1, l2);


            Rectangle _circle = new Rectangle(217, 10, 62, 62);
            g.DrawEllipse(new Pen(Color.White, 2), _circle);
            g.FillEllipse(_style.PointBrush, _circle);


            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            int imgLength = Convert.ToInt32(ms.Length);
            byte[] bytes = new byte[imgLength];
            ms.Read(bytes, 0, imgLength);
            ms.Close();


            return bytes;
        }
    }
}
