namespace LabProgramowanie_II.Models{

    public class SuccessResponse<T> : ApiResponse{
        
        public T result {set;get;}

        public SuccessResponse(T result){
            this.status = "ok";
            this.result = result;
        }

    }

}