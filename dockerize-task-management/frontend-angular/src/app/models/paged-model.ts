export interface PagedModel<T> {
  items: T[];
  currentIndex: number;
  pageSize: number;
  totalCount: number;
  maxPageIndex: number;
}

