using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
namespace AutoCad
{
    /// <summary>
    /// 平台调用类
    /// </summary>
    public class InvokeArx
    {
        private static void AddValueToResultBuffer(ref ResultBuffer rb, object obj)
        {
            if (obj == null)
            {
                rb.Add(new TypedValue((int)LispDataType.Text, ""));
            }
            else
            {
                if (obj is string)
                {
                    rb.Add(new TypedValue((int)LispDataType.Text, obj));
                }
                else if (obj is Point2d)
                {
                    rb.Add(new TypedValue((int)LispDataType.Text, "_non"));
                    rb.Add(new TypedValue((int)LispDataType.Point2d, obj));
                }
                else if (obj is Point3d)
                {
                    rb.Add(new TypedValue((int)LispDataType.Text, "_non"));
                    rb.Add(new TypedValue((int)LispDataType.Point3d, obj));
                }
                else if (obj is ObjectId)
                {
                    rb.Add(new TypedValue((int)LispDataType.ObjectId, obj));
                }
                else if (obj is SelectionSet)
                {
                    rb.Add(new TypedValue((int)LispDataType.SelectionSet, obj));
                }
                else if (obj is double)
                {
                    rb.Add(new TypedValue((int)LispDataType.Double, obj));
                }
                else if (obj is short)
                {
                    rb.Add(new TypedValue((int)LispDataType.Int16, obj));
                }
                else if (obj is int)
                {
                    rb.Add(new TypedValue((int)LispDataType.Int32, obj));
                }
                else if (obj is TypedValue)
                {
                    rb.Add(obj);
                }
            }
        }
        #region Redraw
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 acedRedraw(long[] name, Int32 mode);
        public static void Redraw()
        {
            acedRedraw(null, 1);
        }
        //[DllImport("acdb17.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?acdbGetAdsName@@YA?AW4ErrorStatus@Acad@@AAY01JVAcDbObjectId@@@Z")]
        //private static extern int acdbGetAdsName(long[] name, ObjectId objId);
        //public static void Redraw(ObjectId id)
        //{
        //    long[] adsname = new long[2];
        //    acdbGetAdsName(adsname, id);
        //    acedRedraw(adsname, 1);
        //}
        #endregion
        #region TextScr
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        private static extern int acedTextScr();
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        private static extern int acedGraphScr();
        public static bool DisplayTextScreen
        {
            set
            {
                if (value)
                    acedTextScr();
                else
                    acedGraphScr();
            }
        }
        #endregion
        #region Command
        /// <summary>
        /// 控制命令行回显
        /// </summary>
        public static bool CmdEcho
        {
            get
            {
                return (int)Application.GetSystemVariable("cmdecho") == 1;
            }
            set
            {
                Application.SetSystemVariable("cmdecho", Convert.ToInt16(value));
            }
        }
        [DllImport("acad.exe")]
        private static extern int acedCmd(IntPtr vlist);

        /// <summary>
        /// 调用AutoCad命令
        /// </summary>
        /// <param name="prompt">命令行提示</param>
        /// <param name="arg">参数</param>
        public static void Command(string prompt, object arg)
        {
           // EdUtility.WriteMessage(prompt);
            ResultBuffer rb = new ResultBuffer();
            AddValueToResultBuffer(ref rb, arg);
            try
            {
                acedCmd(rb.UnmanagedObject);
            }
            catch { }
            finally
            {
                rb.Dispose();
            }
        }
        /// <summary>
        /// 调用AutoCad命令
        /// </summary>
        /// <param name="endCommandByUser">命令结束方式</param>
        /// <param name="ldnode">参数</param>
        public static void Command(bool endCommandByUser, ResultBuffer rb)
        {
            ResultBuffer rbend = new ResultBuffer();
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                string currCmdName = doc.CommandInProgress;
                acedCmd(rb.UnmanagedObject);
                if (endCommandByUser)
                {
                    rbend.Add(new TypedValue((int)LispDataType.Text, "\\"));
                }
                while (doc.CommandInProgress != currCmdName)
                {
                    acedCmd(rbend.UnmanagedObject);
                }
            }
            catch { }
            finally
            {
                rb.Dispose();
                rbend.Dispose();
            }
        }
        /// <summary>
        /// 调用AutoCad命令
        /// </summary>
        /// <param name="endCommandByUser">命令结束方式</param>
        /// <param name="args">参数</param>
        public static void Command(bool endCommandByUser, params object[] args)
        {
            ResultBuffer rb = new ResultBuffer();
            ResultBuffer rbend = new ResultBuffer();
            foreach (object val in args)
            {
                AddValueToResultBuffer(ref rb, val);
            }
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                string currCmdName = doc.CommandInProgress;
                acedCmd(rb.UnmanagedObject);
                if (endCommandByUser)
                {
                    rbend.Add(new TypedValue((int)LispDataType.Text,"\\"));
                }
                while (doc.CommandInProgress != currCmdName)
                {
                    acedCmd(rbend.UnmanagedObject);
                }
            }
            catch { }
            finally
            {
                rb.Dispose();
                rbend.Dispose();
            }
        }
        #endregion

        #region Lisp
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        extern static private int acedPutSym(string name, IntPtr result);
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        extern static private int acedGetSym(string name, out IntPtr result);
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("acad.exe", CallingConvention = CallingConvention.Cdecl)]
        extern static private int acedInvoke(IntPtr args, out IntPtr result);
        public static ResultBuffer CallLispFunction(ResultBuffer args)
        {
            IntPtr ip = IntPtr.Zero;
            int st = acedInvoke(args.UnmanagedObject, out ip);
            if (ip != IntPtr.Zero)
            {
                ResultBuffer rbRes = ResultBuffer.Create(ip, true);
                return rbRes;
            }
            return null;
        }
        public static ResultBuffer CallLispFunction(string name, params object[] args)
        {
            ResultBuffer rbArgs = new ResultBuffer();
            rbArgs.Add(new TypedValue((int)LispDataType.Text, name));
            foreach (object val in args)
            {
                AddValueToResultBuffer(ref rbArgs, val);
            }
            return CallLispFunction(rbArgs);
        }

        internal static ResultBuffer GetLispSym(string name)
        {
            IntPtr ip = IntPtr.Zero;
            acedGetSym(name, out ip);
            if (ip != IntPtr.Zero)
            {
                ResultBuffer rbRes = ResultBuffer.Create(ip, true);
                return rbRes;
            }
            return null;
        }
        #endregion
    }
}