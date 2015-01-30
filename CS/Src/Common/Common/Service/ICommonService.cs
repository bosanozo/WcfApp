using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

using Common;

namespace Common.Service
{
    //************************************************************************
    /// <summary>
    /// 共通処理サービスインターフェース
    /// </summary>
    //************************************************************************
    [ServiceContract]
    public interface ICommonService : IBaseService
    {
        //************************************************************************
        /// <summary>
        /// ユーザ情報を取得する。
        /// </summary>
        /// <returns>ユーザ情報</returns>
        //************************************************************************
        [OperationContract]
        UserInformation GetUserInfo();

        //************************************************************************
        /// <summary>
        /// 指定された検索IDのSQLファイルからSELECT文を作成し、検索を実行する。
        /// </summary>
        /// <param name="argSelectIdList">検索IDリスト</param>
        /// <param name="argParamList">検索条件リスト</param>
        /// <param name="argSelectType">検索種別</param>
        /// <param name="argMessage">返却メッセージ</param>
        /// <returns>検索結果</returns>
        //************************************************************************
        [OperationContract]
        [FaultContract(typeof(ApplicationMessage))]
        DataSet Select(List<SelectId> argSelectIdList, List<SelectParam> argParamList,
            SelectType argSelectType, out ApplicationMessage argMessage);

        //************************************************************************
        /// <summary>
        /// データセットのデータを登録する。
        /// </summary>
        /// <param name="argDataSet">登録データ</param>
        /// <param name="argMessage">返却メッセージ</param>
        /// <returns>処理件数</returns>
        //************************************************************************
        [OperationContract]
        [FaultContract(typeof(ApplicationMessage))]
        int Update(DataSet argDataSet, out ApplicationMessage argMessage);
    }
}
