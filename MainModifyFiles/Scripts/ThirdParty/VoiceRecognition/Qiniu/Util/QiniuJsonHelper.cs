//using Newtonsoft.Json;
using Qiniu.RS;

namespace Qiniu.Util
{
	public static class QiniuJsonHelper
	{
		public static string JsonEncode (PutPolicy obj)
		{

			string json = "";

			/// <summary>
			/// 一般指文件要上传到的目标存储空间（Bucket）。若为”Bucket”，表示限定只能传到该Bucket（仅限于新增文件）；若为”Bucket:Key”，表示限定特定的文件，可修改该文件。
			/// </summary>
			//		[JsonProperty("scope")]
			if(!string.IsNullOrEmpty(obj.Scope))
				json +=string.Format("\"scope\":\"{0}\",",obj.Scope);

			/// <summary>
			/// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的URL，必须是公网上可以正常进行POST请求并能响应 HTTP Status 200 OK 的有效 URL
			/// </summary>
			//		[JsonProperty("callBackUrl")]
			if(!string.IsNullOrEmpty(obj.CallBackUrl))
				json +=string.Format("\"callBackUrl\":\"{0}\",",obj.CallBackUrl);

			/// <summary>
			/// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的数据。支持 魔法变量 和 自定义变量，不可与 returnBody 同时使用。
			/// </summary>
			//		[JsonProperty("callBackBody")]
			if(!string.IsNullOrEmpty(obj.CallBackBody))
				json +=string.Format("\"callBackBody\":\"{0}\",",obj.CallBackBody);

			/// <summary>
			/// 设置用于浏览器端文件上传成功后，浏览器执行301跳转的URL，一般为 HTML Form 上传时使用。文件上传成功后会跳转到 returnUrl?query_string, query_string 会包含 returnBody 内容。returnUrl 不可与 callbackUrl 同时使用
			/// </summary>
			//		[JsonProperty("returnUrl")]
			if(!string.IsNullOrEmpty(obj.ReturnUrl))
				json +=string.Format("\"returnUrl\":\"{0}\",",obj.ReturnUrl);

			/// <summary>
			/// 文件上传成功后，自定义从 Qiniu-Cloud-Server 最终返回給终端 App-Client 的数据。支持 魔法变量，不可与 callbackBody 同时使用。
			/// </summary>
			//		[JsonProperty("returnBody")]
			if(!string.IsNullOrEmpty(obj.ReturnBody))
				json +=string.Format("\"returnBody\":\"{0}\",",obj.ReturnBody);

			/// <summary>
			/// 给上传的文件添加唯一属主标识，特殊场景下非常有用，比如根据终端用户标识给图片或视频打水印
			/// </summary>
			//		[JsonProperty("endUser")]
			if(!string.IsNullOrEmpty(obj.EndUser))
				json +=string.Format("\"endUser\":\"{0}\",",obj.EndUser);

			/// <summary>
			/// 定义 uploadToken 的失效时间，Unix时间戳，精确到秒，缺省为 3600 秒
			/// </summary>
			//		[JsonProperty("deadline")]
			json +=string.Format("\"deadline\":{0},",obj.Deadline);

			/// <summary>
			/// 可选, Gets or sets the save key.
			/// </summary>
			/// <value>The save key.</value>
			//		[JsonProperty("saveKey")]
			if(!string.IsNullOrEmpty(obj.SaveKey))
				json +=string.Format("\"saveKey\":\"{0}\",",obj.SaveKey);

			/// <summary>
			/// 可选。 若非0, 即使Scope为 Bucket:Key 的形式也是insert only.
			/// </summary>
			/// <value>The insert only.</value>
			//		[JsonProperty("insertOnly")]
			json +=string.Format("\"insertOnly\":{0},",obj.InsertOnly);

			/// <summary>
			/// 可选。若非0, 则服务端根据内容自动确定 MimeType */
			/// </summary>
			/// <value>The detect MIME.</value>
			//		[JsonProperty("detectMime")]
			json +=string.Format("\"detectMime\":{0},",obj.DetectMime);

			/// <summary>
			/// 限定用户上传的文件类型
			/// 指定本字段值，七牛服务器会侦测文件内容以判断MimeType，再用判断值跟指定值进行匹配，匹配成功则允许上传，匹配失败返回400状态码
			/// 示例:
			///1. “image/*“表示只允许上传图片类型
			///2. “image/jpeg;image/png”表示只允许上传jpg和png类型的图片
			/// </summary>
			/// <value>The detect MIME.</value>
			//        [JsonProperty("mimeLimit")]
			if(!string.IsNullOrEmpty(obj.MimeLimit))
				json +=string.Format("\"mimeLimit\":\"{0}\",",obj.MimeLimit);

			/// <summary>
			/// 可选, Gets or sets the fsize limit.
			/// </summary>
			/// <value>The fsize limit.</value>
			//		[JsonProperty("fsizeLimit")]
			json +=string.Format("\"fsizeLimit\":{0},",obj.FsizeLimit);

			/// <summary>
			/// 音视频转码持久化完成后，七牛的服务器会向用户发送处理结果通知。这里指定的url就是用于接收通知的接口。设置了`persistentOps`,则需要同时设置此字段
			/// </summary>
			//		[JsonProperty("persistentNotifyUrl")]
			if(!string.IsNullOrEmpty(obj.PersistentNotifyUrl))
				json +=string.Format("\"persistentNotifyUrl\":\"{0}\",",obj.PersistentNotifyUrl);

			/// <summary>
			/// 可指定音视频文件上传完成后，需要进行的转码持久化操作。persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
			/// </summary>
			//		[JsonProperty("persistentOps")]
			if(!string.IsNullOrEmpty(obj.PersistentOps))
				json +=string.Format("\"persistentOps\":\"{0}\",",obj.PersistentOps);

			// <summary>
			/// 可指定音视频文件上传后处理的队列，不指定时在公共队列中。persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
			/// </summary>
			//		[JsonProperty("persistentPipeline")]
			if(!string.IsNullOrEmpty(obj.PersistentPipeline))
				json +=string.Format("\"persistentPipeline\":\"{0}\",",obj.PersistentPipeline);

			json = json.Substring(0,json.Length - 1);
			return "{" + json + "}";
		}

		/*
		public static string JsonEncode (object obj)
		{
			JsonSerializerSettings setting = new JsonSerializerSettings ();
			setting.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject (obj, setting);
		}

		public static T ToObject<T> (string value)
		{
			return JsonConvert.DeserializeObject<T> (value);
		}
		*/
	}
}
