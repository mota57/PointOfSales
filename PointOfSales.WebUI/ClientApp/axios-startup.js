export default {
     Config(axios) {
          // Add a request interceptor
        axios.interceptors.request.use(function (config) {
            // Do something before request is sent
            let headers = {};
            //https://github.com/aspnet/AspNetCore/issues/9039
            headers['X-Requested-With'] = 'XMLHttpRequest';
            if (config.headers) {
            Object.assign(config.headers, headers);
            } else {
            config.headers = headers;
            }
        
            return config;
        }, function (error) {
            // Do something with request error
            return Promise.reject(error);
        });
        
        // Add a response interceptor
        axios.interceptors.response.use(function (response) {
            // Any status code that lie within the range of 2xx cause this function to trigger
            // Do something with response data
            return response;
        }, function (error) {
            // Any status codes that falls outside the range of 2xx cause this function to trigger
            // Do something with response error
            console.debug('INTERCEPTOR AXIOS ERROR NEXT LINE')
            console.debug(error)
            return Promise.reject(error);
        });
    } 
  
}
