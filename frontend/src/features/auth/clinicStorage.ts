const clinicKey = "vetcom.auth.activeClinicId";
export function readStoredClinicId(){if(typeof window==="undefined")return null;return window.localStorage.getItem(clinicKey);}
export function storeClinicId(id:string){window.localStorage.setItem(clinicKey,id);}
export function clearStoredClinicId(){window.localStorage.removeItem(clinicKey);}
