/* SystemJS module definition */
declare var module: NodeModule;
interface NodeModule {
  id: string;
}

declare namespace jasmine {
  interface Matchers<T> {
    toHaveCssClass(expected: any): boolean;
  }
}

declare var hljs: any;
declare var wx: any;
declare var JsBarcode: any;
declare var qq: any;
declare var QRCode: any;



