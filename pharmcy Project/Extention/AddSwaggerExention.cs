namespace pharmcy_Project.Extention
{
  public  static class AddSwaggerExention
    {

        public static WebApplication useswaggerMidlleWares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();


            return  app;
        }
    }
}
