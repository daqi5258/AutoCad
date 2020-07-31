using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoCad
{
    public static class layerTools
    {

        public static  ObjectId AddLayerTable(this Database db,string layerName)
        {
            LayerTable lt = db.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
            if (lt.Has(layerName))
            {
                return lt[layerName];
            }
            else
            {
                LayerTableRecord ltr = new LayerTableRecord();
                ltr.Name = layerName;
                lt.UpgradeOpen();
                lt.Add(ltr);
                db.TransactionManager.AddNewlyCreatedDBObject(ltr,true);
                lt.DowngradeOpen();
                return lt[layerName];
            }
        }
    
        public static bool deleteLayerTable(this Database db, string layerName)
        {
            LayerTable lt = db.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
            if (!lt.Has(layerName) || layerName=="0" || layerName=="Defpoints" || db.Clayer ==lt[layerName])
            {
                return false;
            }
            else
            {
                LayerTableRecord ltr = lt[layerName].GetObject(OpenMode.ForRead) as LayerTableRecord;
                lt.GenerateUsageData();
                if (ltr.IsUsed)
                    return false;
                ltr.UpgradeOpen();
                ltr.Erase();
                return true;
            }

        }
        /// <summary>
        /// 设置图层颜色和当前图层
        /// </summary>
        /// <param name="db"></param>
        /// <param name="layerName">图层名</param>
        /// <param name="colorIndex">颜色</param>
        /// <param name="setCtLy">是否设置为当前图层</param>
        /// <returns></returns>
        public static bool setLayer(this Database db,String layerName,short colorIndex,bool setCtLy)
        {
            LayerTable lt = db.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
            if (lt.Has(layerName))
            {
                LayerTableRecord ltr = lt[layerName].GetObject(OpenMode.ForWrite) as LayerTableRecord;
                if (colorIndex >= 0)
                {
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, colorIndex);
                }
                if (setCtLy && db.Clayer != lt[layerName])
                {
                    db.Clayer = lt[layerName];
                }
                return true;
            }
            else
                return false;
        }
   
        public static List<LayerTableRecord> GetLayerTableRecords(this Database db )
        {
            LayerTable lt = db.LayerTableId.GetObject(OpenMode.ForRead) as LayerTable;
            List<LayerTableRecord> ltrs = new List<LayerTableRecord>();
            foreach (ObjectId objId in lt)
            {
                LayerTableRecord ltr = objId.GetObject(OpenMode.ForRead) as LayerTableRecord;
                ltrs.Add(ltr);
            }
            return ltrs;
        }
        
    
    }
}
