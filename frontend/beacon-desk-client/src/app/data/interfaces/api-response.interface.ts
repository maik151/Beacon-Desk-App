
//Clase con la estructura de exito de las respuestas de la API
export interface ApiResponse<T>{
    statusCode: number;
    success: boolean;
    message: string;
    correlationId: string;
    data: T;
}

//// Estándar de error (RFC 7807)
export interface ProblemDetails{
    type: string;
    title: string;
    status: number;
    detail: string;
    instance: string;
    correlationId: string;
}

