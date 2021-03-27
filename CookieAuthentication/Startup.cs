using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CookieAuthentication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		/// <summary>
		/// ���̃��\�b�h�̓����^�C���ɂ���ČĂяo����܂��B���̃��\�b�h���g�p���āA�R���e�i�[�ɃT�[�r�X��ǉ����܂��B
		/// </summary>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			// Cookie �ɂ��F�؃X�L�[����ǉ�����
			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie();

			services.AddAuthorization(options =>
			{
				// AllowAnonymous �������w�肳��Ă��Ȃ����ׂĂ� Action �Ȃǂɑ΂��ă��[�U�[�F�؂��K�v�ƂȂ�
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});
		}

		/// <summary>
		/// ���̃��\�b�h�̓����^�C���ɂ���ČĂяo����܂��B ���̃��\�b�h���g�p���āAHTTP�v���p�C�v���C�����\�����܂��B
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// �f�t�H���g��HSTS�l��30���ł��B�^�p�V�i���I�ł͂����ύX���邱�Ƃ��ł��܂��Bhttps�F//aka.ms/aspnetcore-hsts���Q�Ƃ��Ă��������B
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication(); // [�ǉ�] �F��
			app.UseAuthorization(); // �F��

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
