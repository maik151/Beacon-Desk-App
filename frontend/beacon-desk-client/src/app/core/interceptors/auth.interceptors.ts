import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. Recuperamos el token del almacenamiento
  const token = localStorage.getItem('token');

  // 2. Si existe, clonamos la petición y le pegamos el token
  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq);
  }

  // 3. Si no hay token, dejamos pasar la petición tal cual (ej: login)
  return next(req);
};