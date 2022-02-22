import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'userFilter'
})
export class UserFilterPipe implements PipeTransform {

  transform(items: any[], filter: string): any {
    if (!items || !filter || filter?.length == 0) {
      return items;
    }
    return items.filter(item => item.name.toLowerCase().indexOf(filter.toLowerCase()) !== -1);
  }

}
