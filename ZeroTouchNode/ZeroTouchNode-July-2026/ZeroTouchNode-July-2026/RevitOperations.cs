using Autodesk.Revit.DB;
using Revit.Elements;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Revit.GeometryConversion;

namespace ZeroTouchNode_July_2026
{
    public class RevitOperations
    {
        public static string GetName(Revit.Elements.Element abc)
        {
            return abc.Name;
        }

        public static int GetWallCount()
        {
            //walls = FilteredElementCollector(doc).OfClass(Wall).ToElements()
            //return len(walls)

            var doc = DocumentManager.Instance.CurrentDBDocument;

            var walls = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.Wall)).ToElements();
            var wallCounts = walls.Count;

            return wallCounts;
        }


        public static Revit.Elements.Element CreateWall(Revit.Elements.Element DetailLine)
        {
            var revitElement = DetailLine.InternalElement;

            var doc = DocumentManager.Instance.CurrentDBDocument;

            TransactionManager.Instance.EnsureInTransaction(doc);

            var locCurve = revitElement.Location as LocationCurve;
            var curve = locCurve.Curve;

            var levels = new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.Level)).ToElements();

            Autodesk.Revit.DB.Level myLevel = null;

            foreach (Autodesk.Revit.DB.Level level in levels)
            {
                if (level.Name == "Level 0")
                    myLevel = level;
            }

            var newWall = Autodesk.Revit.DB.Wall.Create(doc, curve, myLevel.Id, false);

            TransactionManager.Instance.TransactionTaskDone();

            return newWall.ToDSType(true);
        }

    }
}
