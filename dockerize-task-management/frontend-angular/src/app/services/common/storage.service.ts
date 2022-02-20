import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class StorageService {
    getValue(key: string) {
        return localStorage[key] || null;
    }

    setValue(key: string, value: string) {
        localStorage[key] = value;
    }

    remove(key: string) {
        localStorage.removeItem(key);
    }
}