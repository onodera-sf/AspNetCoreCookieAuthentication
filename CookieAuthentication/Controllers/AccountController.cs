using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuthentication.Controllers
{
	/// <remarks>
	/// <see cref="AllowAnonymous"/> 属性は Cookie 認証していなくてもアクセスできる Action (Controller) であることを示す。
	/// </remarks>
	[AllowAnonymous]
	public class AccountController : Controller
	{
		/// <summary>仮のユーザーデータベースとする。</summary>
		private Dictionary<string, string> UserAccounts { get; set; }

		public AccountController()
		{
			// 仮のユーザーを登録する
			UserAccounts = new Dictionary<string, string>
			{
				{ "user1", "password1" },
				{ "user2", "password2" },
			};
		}

		/// <summary>ログイン画面を表示します。</summary>
		public IActionResult Login()
		{
			return View();
		}

		/// <summary>ログイン処理を実行します。</summary>
		[HttpPost]
		public async Task<IActionResult> Login(string userName, string password)
		{
			// ユーザーの存在チェックとパスワードチェック (仮実装)
			// 本 Tips は Cookie 認証ができるかどうかの確認であるため入力内容やパスワードの厳密なチェックは行っていません
			if (UserAccounts.TryGetValue(userName, out string getPass) == false || password != getPass)
			{
				return View();
			}

			// サインインに必要なプリンシパルを作る
			var claims = new[] { new Claim(ClaimTypes.Name, userName) };
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			// 認証クッキーをレスポンスに追加
			await HttpContext.SignInAsync(principal);

			// ログインが必要な画面にリダイレクトします
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}


		/// <summary>ログアウト処理を実行します。</summary>
		public async Task<IActionResult> Logout()
		{
			// 認証クッキーをレスポンスから削除
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// ログイン画面にリダイレクト
			return RedirectToAction(nameof(Login));
		}
	}
}
