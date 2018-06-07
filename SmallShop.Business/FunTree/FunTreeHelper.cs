using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace SmallShop.BackStage.Business
{
    public static class FunTreeHelper
    {
        private static readonly string FunTree_Key = "Game_FunTree_Key";

        /// <summary>
        /// 系统功能树
        /// </summary>
        public static FunTree FunTree
        {
            get
            {
                var tree = CacheHelper.Instance.Get<FunTree>(FunTree_Key);
                if (tree == null)
                {
                    lock (FunTree_Key)
                    {
                        tree = CacheHelper.Instance.Get<FunTree>(FunTree_Key);
                        if (tree == null)
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(FunTree));
                            var path = HttpContext.Current.Server.MapPath("~/App_Data/FunTree.xml");
                            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                            {
                                try
                                {
                                    tree = ser.Deserialize(fs) as FunTree;
                                }
                                catch (Exception)
                                {
                                    throw new Exception("功能树初始化失败");
                                }
                            }
                            CacheHelper.Instance.Add(FunTree_Key, tree);
                        }
                    }
                }
                return tree;
            }
        }
    }
}
