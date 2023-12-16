
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Bunifu.UI.WinForms;


namespace Sign_in_Application
{
    public partial class Form3 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True";
        private bool isEditing = false;
        string name;

        public Form3(string name)
        {
            InitializeComponent();
            DisplayAllProductsOnPanel();
            DisplaySalesDataOnPanel();
            ProductDGV.CellEndEdit += ProductDGV_CellEndEdit;
            DisplayProductCategories();
            this.name = name;
            LoadCategories();
            LoadProducts();
            ProductsDropdown1.SelectedIndexChanged += ProductsDropdown1_SelectedIndexChanged;



        }

        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string editColumnName = "Edit";
            string deleteColumnName = "Delete";

            if (e.RowIndex >= 0)
            {
                int editColumnIndex = -1;
                int deleteColumnIndex = -1;

                // Find the column indices by name
                foreach (DataGridViewColumn column in ProductDGV.Columns)
                {
                    if (column.HeaderText.Equals(editColumnName))
                        editColumnIndex = column.Index;
                    else if (column.HeaderText.Equals(deleteColumnName))
                        deleteColumnIndex = column.Index;
                }

                if (e.ColumnIndex == editColumnIndex && e.RowIndex >= 0)
                {
                    // Handle edit button click for the selected row
                    int productId = Convert.ToInt32(ProductDGV.Rows[e.RowIndex].Cells["product_id"].Value);
                    // Call a method to handle edit operation based on productId
                    HandleEditProduct(productId);
                }
                else if (e.ColumnIndex == deleteColumnIndex && e.RowIndex >= 0)
                {
                    // Handle delete button click for the selected rows
                    List<int> selectedProductIds = new List<int>();

                    foreach (DataGridViewRow row in ProductDGV.SelectedRows)
                    {
                        int productId = Convert.ToInt32(row.Cells["product_id"].Value);
                        selectedProductIds.Add(productId);
                    }

                    HandleDeleteProducts(selectedProductIds);
                }
            }
        }




        private void HandleEditProduct(int productId)

        {

            if (ProductDGV.SelectedRows.Count > 0)
            {
                // Display the information of the first selected row in textboxes
                DataGridViewRow firstSelectedRow = ProductDGV.SelectedRows[0];
                productCategoryComboBox.Text = firstSelectedRow.Cells["product_name"].Value.ToString();
                QuantityTb.Text = firstSelectedRow.Cells["stock_quantity"].Value.ToString();
                PriceTb.Text = firstSelectedRow.Cells["unit_price"].Value.ToString();
                // Assuming dropdown is your ComboBox or DropDownList
                dropdown.Text = firstSelectedRow.Cells["category_name"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a product to edit.");
            }
        }

        private void HandleDeleteProducts(List<int> productIds)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected products?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        foreach (int productId in productIds)
                        {
                            string checkStockQuery = "SELECT COUNT(*) FROM stock WHERE product_id = @ProductId";
                            using (SqlCommand checkStockCmd = new SqlCommand(checkStockQuery, conn))
                            {
                                checkStockCmd.Parameters.AddWithValue("@ProductId", productId);

                                int stockRecordCount = (int)checkStockCmd.ExecuteScalar();

                                if (stockRecordCount > 0)
                                {
                                    string deleteStockQuery = "DELETE FROM stock WHERE product_id = @ProductId";
                                    using (SqlCommand deleteStockCmd = new SqlCommand(deleteStockQuery, conn))
                                    {
                                        deleteStockCmd.Parameters.AddWithValue("@ProductId", productId);
                                        deleteStockCmd.ExecuteNonQuery();
                                    }
                                }
                                string deleteProductQuery = "DELETE FROM products WHERE product_id = @ProductId";
                                using (SqlCommand deleteProductCmd = new SqlCommand(deleteProductQuery, conn))
                                {
                                    deleteProductCmd.Parameters.AddWithValue("@ProductId", productId);
                                    deleteProductCmd.ExecuteNonQuery();
                                }
                                string deleteCategoryQuery = "DELETE FROM product_categories WHERE product_category_id NOT IN (SELECT product_category_id FROM products)";
                                using (SqlCommand deleteCategoryCmd = new SqlCommand(deleteCategoryQuery, conn))
                                {
                                    deleteCategoryCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show("Selected products have been successfully deleted.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    DisplayAllProductsOnPanel();
                }
            }
        }

        private void DisplayAllProductsOnPanel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string getAllProductsQuery = "SELECT p.product_id, p.product_name, p.product_category_id, c.category_name, s.unit_price, s.stock_quantity " +
                                                 "FROM products p " +
                                                 "INNER JOIN product_categories c ON p.product_category_id = c.product_category_id " +
                                                 "INNER JOIN stock s ON p.product_id = s.product_id";
                    using (SqlCommand getAllProductsCmd = new SqlCommand(getAllProductsQuery, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(getAllProductsCmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Add edit and delete buttons
                        DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                        editButton.HeaderText = "Edit";
                        editButton.Text = "Edit";
                        editButton.UseColumnTextForButtonValue = true;

                        DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Delete";
                        deleteButton.Text = "Delete";
                        deleteButton.UseColumnTextForButtonValue = true;

                        // Clear existing columns and data
                        ProductDGV.Columns.Clear();
                        ProductDGV.DataSource = null;

                        // Bind the data to the DataGridView
                        ProductDGV.DataSource = dataTable;
                        BprodDGV.DataSource = dataTable;

                        // Add the columns to the end of the DataGridView columns
                        ProductDGV.Columns.Add(editButton);
                        ProductDGV.Columns.Add(deleteButton);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching products: " + ex.Message);
            }
        }
        private bool IsProductNameExists(string productName, int excludeProductId = -1)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkProductNameQuery = "SELECT COUNT(*) FROM products WHERE product_name = @ProductName AND product_id <> @ExcludeProductId";
                    using (SqlCommand checkProductNameCmd = new SqlCommand(checkProductNameQuery, conn))
                    {
                        checkProductNameCmd.Parameters.AddWithValue("@ProductName", productName);
                        checkProductNameCmd.Parameters.AddWithValue("@ExcludeProductId", excludeProductId);
                        int count = (int)checkProductNameCmd.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking product name: " + ex.Message);
                return false;
            }
        }
        private void DeleteSelectedProduct()
        {
            try
            {
                if (ProductDGV.SelectedRows.Count > 0)
                {
                    int selectedProductId = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells["product_id"].Value);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteStockQuery = "DELETE FROM stock WHERE product_id = @ProductId";
                        using (SqlCommand deleteStockCmd = new SqlCommand(deleteStockQuery, conn))
                        {
                            deleteStockCmd.Parameters.AddWithValue("@ProductId", selectedProductId);
                            deleteStockCmd.ExecuteNonQuery();
                        }
                        string deleteProductQuery = "DELETE FROM products WHERE product_id = @ProductId";
                        using (SqlCommand deleteProductCmd = new SqlCommand(deleteProductQuery, conn))
                        {
                            deleteProductCmd.Parameters.AddWithValue("@ProductId", selectedProductId);
                            deleteProductCmd.ExecuteNonQuery();
                        }
                        string deleteCategoryQuery = "DELETE FROM product_categories WHERE product_category_id NOT IN (SELECT product_category_id FROM products)";
                        using (SqlCommand deleteCategoryCmd = new SqlCommand(deleteCategoryQuery, conn))
                        {
                            deleteCategoryCmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Selected product has been deleted.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a product to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {

                DisplayAllProductsOnPanel();
            }
        }

        private void ProductDGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isEditing)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        int productId = Convert.ToInt32(ProductDGV.Rows[e.RowIndex].Cells["product_id"].Value);
                        string productName = ProductDGV.Rows[e.RowIndex].Cells["product_name"].Value.ToString();
                        int productCategoryId = Convert.ToInt32(ProductDGV.Rows[e.RowIndex].Cells["product_category_id"].Value);
                        string category = ProductDGV.Rows[e.RowIndex].Cells["category_name"].Value.ToString();
                        decimal unitPrice = Convert.ToDecimal(ProductDGV.Rows[e.RowIndex].Cells["unit_price"].Value);
                        int stockQuantity = Convert.ToInt32(ProductDGV.Rows[e.RowIndex].Cells["stock_quantity"].Value);
                        if (IsProductNameExists(productName, productId))
                        {
                            MessageBox.Show("Product name already exists. Please choose a different name.");
                            return;
                        }
                        string updateProductQuery = "UPDATE products SET product_name = @ProductName, product_category_id = @ProductCategoryId WHERE product_id = @ProductId";
                        using (SqlCommand updateProductCmd = new SqlCommand(updateProductQuery, conn))
                        {
                            updateProductCmd.Parameters.AddWithValue("@ProductName", productName);
                            updateProductCmd.Parameters.AddWithValue("@ProductCategoryId", productCategoryId);
                            updateProductCmd.Parameters.AddWithValue("@ProductId", productId);
                            updateProductCmd.ExecuteNonQuery();
                        }
                        string updateStockQuery = "UPDATE stock SET unit_price = @UnitPrice, stock_quantity = @StockQuantity WHERE product_id = @ProductId";
                        using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn))
                        {
                            updateStockCmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                            updateStockCmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                            updateStockCmd.Parameters.AddWithValue("@ProductId", productId);
                            updateStockCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product updated successfully.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the product: " + ex.Message);
                }
                finally
                {
                    DisplayAllProductsOnPanel();
                    isEditing = false;
                }
            }
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (productCategoryComboBox.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || dropdown.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        if (IsProductNameExists(productCategoryComboBox.Text))
                        {
                            MessageBox.Show("Product name already exists. Please choose a different name.");
                            return;
                        }

                        int productCategoryId = GetCategoryId(conn, productCategoryComboBox.Text);

                        if (productCategoryId == -1)
                        {
                            // Category does not exist, insert it
                            string insertCategoryQuery = "INSERT INTO product_categories (category_name) OUTPUT INSERTED.product_category_id VALUES (@CategoryName)";

                            using (SqlCommand insertCategoryCmd = new SqlCommand(insertCategoryQuery, conn))
                            {
                                insertCategoryCmd.Parameters.AddWithValue("@CategoryName", dropdown.Text);
                                productCategoryId = (int)insertCategoryCmd.ExecuteScalar();
                            }
                        }

                        string productQuery = "INSERT INTO products (product_name, product_category_id, input_by, input_dt) " +
                                              "VALUES (@ProductName, @ProductCategoryID, @InputBy, @InputDt); SELECT SCOPE_IDENTITY();";
                        int productId;

                        using (SqlCommand productCmd = new SqlCommand(productQuery, conn))
                        {
                            productCmd.Parameters.AddWithValue("@ProductName", productCategoryComboBox.Text);
                            productCmd.Parameters.AddWithValue("@ProductCategoryID", productCategoryId);
                            productCmd.Parameters.AddWithValue("@InputBy", name);
                            productCmd.Parameters.AddWithValue("@InputDt", DateTime.Now);

                            productId = Convert.ToInt32(productCmd.ExecuteScalar());
                        }

                        string stockQuery = "INSERT INTO stock (unit_price, stock_quantity, product_id) " +
                                            "VALUES (@UnitPrice, @StockQuantity, @ProductID)";
                        using (SqlCommand stockCmd = new SqlCommand(stockQuery, conn))
                        {
                            stockCmd.Parameters.AddWithValue("@UnitPrice", PriceTb.Text);
                            stockCmd.Parameters.AddWithValue("@StockQuantity", QuantityTb.Text);
                            stockCmd.Parameters.AddWithValue("@ProductID", productId);
                            stockCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Data has been successfully inserted.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    DisplayAllProductsOnPanel();
                }
            }
        }
        private int GetCategoryId(SqlConnection connection, string categoryName)
        {
            string query = "SELECT product_category_id FROM product_categories WHERE category_name = @CategoryName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", categoryName);
                object result = command.ExecuteScalar();

                return (result != null) ? Convert.ToInt32(result) : -1;
            }
        }
        private void UpdateProduct()
        {
            if (productCategoryComboBox.Text == "" || QuantityTb.Text == "" || PriceTb.Text == "" || dropdown.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();


                        if (ProductDGV.SelectedRows.Count > 0)
                        {
                            int productIdToUpdate = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells["product_id"].Value);


                            string newProductName = productCategoryComboBox.Text;
                            if (IsProductNameExists(newProductName, productIdToUpdate))
                            {
                                MessageBox.Show("Product name already exists. Please choose a different name.");
                                return;
                            }

                            string updateProductQuery = "UPDATE products SET product_name = @ProductName, " +
                                                        "input_by = @InputBy, input_dt = @InputDt " +
                                                        "WHERE product_id = @ProductId";

                            using (SqlCommand updateProductCmd = new SqlCommand(updateProductQuery, conn))
                            {
                                updateProductCmd.Parameters.AddWithValue("@ProductName", newProductName);
                                updateProductCmd.Parameters.AddWithValue("@InputBy", name);
                                updateProductCmd.Parameters.AddWithValue("@InputDt", DateTime.Now);
                                updateProductCmd.Parameters.AddWithValue("@ProductId", productIdToUpdate);

                                updateProductCmd.ExecuteNonQuery();
                            }

                            string updateStockQuery = "UPDATE stock SET unit_price = @UnitPrice, " +
                                                      "stock_quantity = @StockQuantity " +
                                                      "WHERE product_id = @ProductId";

                            using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn))
                            {
                                updateStockCmd.Parameters.AddWithValue("@UnitPrice", PriceTb.Text);
                                updateStockCmd.Parameters.AddWithValue("@StockQuantity", QuantityTb.Text);
                                updateStockCmd.Parameters.AddWithValue("@ProductId", productIdToUpdate);

                                updateStockCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Data has been successfully edited.");
                        }
                        else
                        {
                            MessageBox.Show("Please select a product to update.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {

                    DisplayAllProductsOnPanel();
                }
            }
        }

        private void UpdateProductQuantity()
        {
            if (QuantityTb.Text == "")
            {
                MessageBox.Show("Please enter a quantity.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    if (ProductDGV.SelectedRows.Count > 0)
                    {
                        int productIdToUpdate = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells["product_id"].Value);


                        int currentStockQuantity;
                        string getStockQuantityQuery = "SELECT stock_quantity FROM stock WHERE product_id = @ProductId";

                        using (SqlCommand getStockQuantityCmd = new SqlCommand(getStockQuantityQuery, conn))
                        {
                            getStockQuantityCmd.Parameters.AddWithValue("@ProductId", productIdToUpdate);
                            currentStockQuantity = Convert.ToInt32(getStockQuantityCmd.ExecuteScalar());
                        }


                        int userEnteredQuantity = Convert.ToInt32(QuantityTb.Text);


                        int newStockQuantity = currentStockQuantity + userEnteredQuantity;


                        string updateStockQuery = "UPDATE stock SET stock_quantity = @StockQuantity " +
                                                  "WHERE product_id = @ProductId";

                        using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn))
                        {
                            updateStockCmd.Parameters.AddWithValue("@StockQuantity", newStockQuantity);
                            updateStockCmd.Parameters.AddWithValue("@ProductId", productIdToUpdate);

                            updateStockCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Product quantity has been updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Please select a product to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {

                DisplayAllProductsOnPanel();
            }
        }


        private void DisplayProductCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DataTable dataTable = FetchProductCategoriesData(conn);
                    CategoryDGV.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching product categories: " + ex.Message);
            }
        }



        private DataTable FetchProductCategoriesData(SqlConnection connection)
        {
            string getCategoryQuery = "SELECT product_category_id, category_name FROM product_categories";

            using (SqlCommand getCategoryCmd = new SqlCommand(getCategoryQuery, connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(getCategoryCmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }


        private void DeleteCategory()
        {
            if (CategoryDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a category to delete.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int categoryIdToDelete = Convert.ToInt32(CategoryDGV.SelectedRows[0].Cells["product_category_id"].Value);


                    if (IsCategoryInUse(categoryIdToDelete))
                    {
                        MessageBox.Show("Cannot delete the category because it has associated products.");
                        return;
                    }


                    string deleteCategoryQuery = "DELETE FROM product_categories WHERE product_category_id = @CategoryId";

                    using (SqlCommand deleteCategoryCmd = new SqlCommand(deleteCategoryQuery, conn))
                    {
                        deleteCategoryCmd.Parameters.AddWithValue("@CategoryId", categoryIdToDelete);
                        deleteCategoryCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Category deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {

                DisplayProductCategories();
            }
        }

        private bool IsCategoryInUse(int categoryId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkCategoryInUseQuery = "SELECT COUNT(*) FROM products WHERE product_category_id = @CategoryId";

                    using (SqlCommand checkCategoryInUseCmd = new SqlCommand(checkCategoryInUseQuery, conn))
                    {
                        checkCategoryInUseCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        int count = (int)checkCategoryInUseCmd.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking if the category is in use: " + ex.Message);
                return true;
            }
        }
        private void AddNewCategory()
        {
            if (string.IsNullOrWhiteSpace(CatNameTb.Text))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the category already exists
                    if (IsCategoryNameExists(CatNameTb.Text, conn))
                    {
                        MessageBox.Show("Category already exists. Please choose a different name.");
                        return;
                    }

                    // Insert new category into product_categories table
                    string insertCategoryQuery = "INSERT INTO product_categories (category_name) VALUES (@CategoryName)";

                    using (SqlCommand insertCategoryCmd = new SqlCommand(insertCategoryQuery, conn))
                    {
                        insertCategoryCmd.Parameters.AddWithValue("@CategoryName", CatNameTb.Text);
                        insertCategoryCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("New category added successfully.");
                    LoadCategories();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                DisplayProductCategories();
            }
        }



        private BunifuTextBox productCategoryBunifuTextBox;



        private bool IsCategoryNameExists(string categoryName, SqlConnection conn)
        {
            try
            {
                string checkCategoryExistsQuery = "SELECT COUNT(*) FROM product_categories WHERE category_name = @CategoryName";

                using (SqlCommand checkCategoryExistsCmd = new SqlCommand(checkCategoryExistsQuery, conn))
                {
                    checkCategoryExistsCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    int count = (int)checkCategoryExistsCmd.ExecuteScalar();

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking if the category name exists: " + ex.Message);
                return true;
            }
        }


        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT product_category_id, category_name FROM product_categories";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);
                                dropdown.DataSource = dataTable;
                                dropdown.DisplayMember = "category_name";
                                dropdown.ValueMember = "product_category_id";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertCustomer(string firstName, string lastName, string phoneNumber)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Please enter valid customer information.");
                return;
            }



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the customer with the given name already exists
                    if (IsCustomerNameExists(conn, firstName, lastName))
                    {
                        MessageBox.Show("Customer with this name already exists. Please choose a different name.");
                        return;
                    }

                    string insertCustomerQuery = "INSERT INTO customers (first_name, last_name, phone_no, input_by, input_dt) " +
                                                 "VALUES (@FirstName, @LastName, @PhoneNumber, @InputBy, @InputDt)";

                    using (SqlCommand insertCustomerCmd = new SqlCommand(insertCustomerQuery, conn))
                    {
                        insertCustomerCmd.Parameters.AddWithValue("@FirstName", firstName);
                        insertCustomerCmd.Parameters.AddWithValue("@LastName", lastName);
                        insertCustomerCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        insertCustomerCmd.Parameters.AddWithValue("@InputBy", name);
                        insertCustomerCmd.Parameters.AddWithValue("@InputDt", DateTime.Now);

                        insertCustomerCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Customer data has been successfully inserted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                Console.WriteLine(ex.ToString()); // Log detailed exception information
            }
            finally
            {
                // You may perform additional actions or refresh the customer data if needed
                DisplayAllCustomersOnPanel();
            }
        }



        private bool IsCustomerNameExists(SqlConnection conn, string firstName, string lastName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkCustomerQuery = "SELECT COUNT(*) FROM customers WHERE first_name = @FirstName AND last_name = @LastName";

                    using (SqlCommand checkCustomerCmd = new SqlCommand(checkCustomerQuery, connection))
                    {
                        checkCustomerCmd.Parameters.AddWithValue("@FirstName", firstName);
                        checkCustomerCmd.Parameters.AddWithValue("@LastName", lastName);

                        int customerCount = (int)checkCustomerCmd.ExecuteScalar();

                        return customerCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking customer existence: " + ex.Message);
                return false;
            }
        }



        private void DisplayAllCustomersOnPanel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string getAllCustomersQuery = "SELECT customer_id, first_name, last_name, phone_no FROM customers";

                    using (SqlCommand getAllCustomersCmd = new SqlCommand(getAllCustomersQuery, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(getAllCustomersCmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear existing columns and data
                        CustomersDGV.Columns.Clear();
                        CustomersDGV.DataSource = null;

                        // Bind the data to the DataGridView
                        CustomersDGV.DataSource = dataTable;

                        // Add delete button column
                        DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Delete";
                        deleteButton.Text = "Delete";
                        deleteButton.UseColumnTextForButtonValue = true;
                        deleteButton.Name = "Delete"; // Set the column name

                        // Add the delete button column to the end of the DataGridView columns
                        CustomersDGV.Columns.Add(deleteButton);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching customers: " + ex.Message);
            }
        }


        private void DisplaySalesDataOnPanel()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT s.sales_id, s.sales_quantity, s.sales_amount, s.sales_date, c.customer_id, p.product_id " +
                                   "FROM sales s " +
                                   "JOIN customers c ON s.customer_id = c.customer_id " +
                                   "JOIN products p ON s.product_id = p.product_id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Clear existing columns and data
                        BillingListDGV.Columns.Clear();
                        BillingListDGV.DataSource = null;

                        // Bind the data to the DataGridView
                        BillingListDGV.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching sales data: " + ex.Message);
            }
        }
        private decimal GetProductPrice(string productName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT s.unit_price " +
                                   "FROM stock s " +
                                   "JOIN products p ON s.product_id = p.product_id " +
                                   "WHERE p.product_name = @ProductName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching product price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0; // Default to 0 if there's an error or no result
        }
        // Example declaration in your class



        private void ProductsDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProductName = ProductsDropdown1.Text;
            decimal productPrice = GetProductPrice(selectedProductName);

            // Display the product price in PriceTextBox1
            PriceTextBox1.Text = $"Rs. {productPrice:N2}";
        }






        private void DeleteSelectedCustomer()
        {
            try
            {
                if (CustomersDGV.SelectedRows.Count > 0)
                {
                    int selectedCustomerId = Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells["customer_id"].Value);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteCustomerQuery = "DELETE FROM customers WHERE customer_id = @CustomerId";
                        using (SqlCommand deleteCustomerCmd = new SqlCommand(deleteCustomerQuery, conn))
                        {
                            deleteCustomerCmd.Parameters.AddWithValue("@CustomerId", selectedCustomerId);
                            deleteCustomerCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Selected customer has been deleted.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a customer to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // You may perform additional actions or refresh the customer data if needed
                DisplayAllCustomersOnPanel();
            }
        }






        private void HandleDeleteCustomer(int customerId)
        {
            DialogResult result = MessageBox.Show($"Are you sure you want to delete Customer ID: {customerId}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Add a breakpoint here by clicking in the left margin
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Delete from customers table
                        string deleteCustomerQuery = "DELETE FROM customers WHERE customer_id = @CustomerId";
                        using (SqlCommand deleteCustomerCmd = new SqlCommand(deleteCustomerQuery, conn))
                        {
                            deleteCustomerCmd.Parameters.AddWithValue("@CustomerId", customerId);
                            deleteCustomerCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Customer has been successfully deleted.");

                        // Refresh the displayed customers after deletion
                        DisplayAllCustomersOnPanel();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT product_id, product_name FROM products";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Assuming ProductsDropdown1 is your ComboBox
                                ProductsDropdown1.DataSource = dataTable;
                                ProductsDropdown1.DisplayMember = "product_name";
                                ProductsDropdown1.ValueMember = "product_id";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }












        private void Grandtotal_Click(object sender, EventArgs e)
        {
            CalculateGrandTotal();
        }



        private void CalculateGrandTotal()
        {
            decimal grandTotal = 0;

            foreach (DataGridViewRow row in YourBillDGV.Rows)
            {
                decimal price = Convert.ToDecimal(row.Cells[2].Value); // Assuming the price is in the third column
                int quantity = Convert.ToInt32(row.Cells[3].Value);    // Assuming the quantity is in the fourth column

                grandTotal += price * quantity;
            }

            // Display the grand total in Pakistani Rupees on a label
            Grandtotal.Text = $"Grand Total: Rs. {grandTotal:N2}";
        }







        // Assuming a class to represent a bill item
        public class BillItem
        {
            public int ID { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
            public int sales_id { get; set; }
        }
        private void RefreshBillDGV()
        {
            try
            {
                // Assuming YourBillDGV is your DataGridView for displaying the bill
                YourBillDGV.Rows.Clear();
                // You may need to re-add the header columns if necessary

                // You can rebind the data or perform any other necessary actions
                // Call the function to display the updated bill
                DisplayAllCustomersOnPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while refreshing the bill: " + ex.Message);
            }
        }

        //       
        private Dictionary<string, int> phoneToCustomerIdMap = new Dictionary<string, int>();

        private int GetCustomerIdByPhoneNumber(string phoneNumber, string customerFname, string customerLname)
        {
            if (phoneToCustomerIdMap.ContainsKey(phoneNumber))
            {
                return phoneToCustomerIdMap[phoneNumber];
            }

            int customerId = -1; // Initialize with a default value

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the customer with the given phone number exists
                    string checkCustomerQuery = "SELECT customer_id FROM customers WHERE phone_no = @PhoneNumber";
                    using (SqlCommand checkCustomerCmd = new SqlCommand(checkCustomerQuery, conn))
                    {
                        checkCustomerCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        // Execute the query to get the customer ID
                        object result = checkCustomerCmd.ExecuteScalar();

                        if (result != null)
                        {
                            // Customer exists, set the customer ID
                            customerId = Convert.ToInt32(result);
                        }
                        else
                        {
                            // If the customer does not exist, insert a new customer into the customers table
                            string insertCustomerQuery = "INSERT INTO customers (phone_no, first_name, last_name, input_by, input_dt) VALUES (@PhoneNumber, @CustomerFname, @CustomerLname, @InputBy, @InputDt); SELECT SCOPE_IDENTITY()";
                            using (SqlCommand insertCustomerCmd = new SqlCommand(insertCustomerQuery, conn))
                            {
                                insertCustomerCmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                                insertCustomerCmd.Parameters.AddWithValue("@CustomerFname", customerFname);
                                insertCustomerCmd.Parameters.AddWithValue("@CustomerLname", customerLname);
                                insertCustomerCmd.Parameters.AddWithValue("@InputBy", name);
                                insertCustomerCmd.Parameters.AddWithValue("@InputDt", DateTime.Now);

                                // Execute the query to insert a new customer and get the generated ID
                                object newCustomerId = insertCustomerCmd.ExecuteScalar();

                                if (newCustomerId != null)
                                {
                                    // Set the customer ID to the newly generated ID
                                    customerId = Convert.ToInt32(newCustomerId);
                                }
                            }
                        }
                    }

                    // Cache the association between phone number and customer ID
                    phoneToCustomerIdMap[phoneNumber] = customerId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking customer phone number: " + ex.Message);
            }

            return customerId;
        }



        private int GenerateCustomerId()
        {
            // This method can be modified based on your specific requirements
            return YourBillDGV.Rows.Count + 1;
        }

        private int GetProductIDByName(string productName)
        {
            int productID = 0;

            // Replace "your_connection_string" with the actual connection string for your database
            //string connectionString = "your_connection_string";

            string query = "SELECT product_id FROM Products WHERE product_name = @product_name";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_name", productName);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out productID))
                    {
                        // Product ID retrieved successfully
                    }
                    else
                    {
                        // Handle the case where the product ID couldn't be retrieved
                        // You might want to log an error or take appropriate action
                    }
                }
            }

            return productID;
        }





        private int GetAvailableQuantityInStock(int productID)
        {
            int availableQuantity = 0;

            // Replace "your_connection_string" with the actual connection string for your database
            // string connectionString = "your_connection_string";

            string query = "SELECT stock_quantity FROM stock WHERE product_id = @product_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", productID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out availableQuantity))
                    {
                        // Quantity available in stock
                    }
                    else
                    {
                        // Handle the case where the quantity couldn't be retrieved
                        // You might want to log an error or take appropriate action
                    }
                }
            }

            return availableQuantity;
        }






        private int GetProductIdByName(string productName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT product_id FROM products WHERE product_name = @ProductName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            // Handle the case where the product ID is not found
                            MessageBox.Show("Product ID not found for the given product name.");
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while getting product ID by name: " + ex.Message);
                return -1;
            }
        }
        private decimal CalculateSalesAmount(int quantity, decimal price)
        {
            return quantity * price;
        }
        private BindingSource bindingSource;

        private void DisplayDataOnDGV()
        {
            try
            {
                // Fetch data from the sales table
                string selectQuery = "SELECT sales_id, sales_quantity, sales_amount, sales_date, customer_id, product_id, input_by, input_dt FROM sales";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Use a SqlDataAdapter to fill a DataTable with the results of the query
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, conn))
                    {
                        dataAdapter.Fill(dataTable);
                    }

                    // Use a BindingSource to update the DataGridView
                    bindingSource = new BindingSource();
                    bindingSource.DataSource = dataTable;

                    // Set the DataGridView data source to the BindingSource
                    BillingListDGV.DataSource = bindingSource;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while fetching data: " + ex.Message);
            }
        }











        private void AddBillBtn_Click(object sender, EventArgs e)
        {
            string customerPhoneNumber = CustomerPhoneNumberTb.Text;

            // Get the customer ID based on the phone number or generate a new one
            int customerId = GetCustomerIdByPhoneNumber(customerPhoneNumber, CustomerFnameTb.Text, CustLnameTb.Text);

            // Assuming you have a class to represent a bill item
            BillItem billItem = new BillItem
            {
                ID = customerId,
                ProductName = ProductsDropdown1.Text,
                Price = GetProductPrice(ProductsDropdown1.Text),
                Quantity = Convert.ToInt32(QuanTB.Text)
            };

            // Get the product ID based on the product name
            int productId = GetProductIDByName(billItem.ProductName);

            // Check if the quantity is available in stock
            int availableQuantityInStock = GetAvailableQuantityInStock(productId);
            if (billItem.Quantity > availableQuantityInStock)
            {
                MessageBox.Show("Not enough stock available for the selected product.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Do not proceed further
            }

            // Continue with the rest of your existing code...
            // For example, updating the DataGridView or performing other actions

            // Check if the product is already in the DataGridView
            bool productFound = false;
            int salesID = 0; // Initialize the sales ID

            foreach (DataGridViewRow row in YourBillDGV.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == billItem.ProductName)
                {
                    // Product found, update the quantity
                    int newQuantity = Convert.ToInt32(row.Cells[3].Value) + billItem.Quantity;
                    row.Cells[3].Value = newQuantity;
                    productFound = true;

                    // Update stock_quantity in the "stock" table
                    //UpdateStockQuantity(productId, newQuantity);

                    // Use the existing sales_id associated with the item
                    salesID = Convert.ToInt32(row.Cells[0].Value);

                    break;
                }
            }

            // If the product is not found, add a new row
            if (!productFound)
            {
                // Use billItem.ID as the sales_id for this transaction
                YourBillDGV.Rows.Add(billItem.ID, billItem.ProductName, billItem.Quantity, billItem.Price, billItem.Price * billItem.Quantity);

                // Update stock_quantity in the "stock" table
                //UpdateStockQuantity(productId, billItem.Quantity);
            }

            // Save the bill using the generated sales_id
            //SaveBill(customerId, productId, billItem.Quantity, billItem.Price * billItem.Quantity, salesID);

            CalculateGrandTotal();
        }


        private void SaveBill(int id, string productName, int quantity, decimal amount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand enableIdentityCmd = new SqlCommand("SET IDENTITY_INSERT sales ON", conn))
                    {
                        enableIdentityCmd.ExecuteNonQuery();
                    }


                    // Find customer_id based on phone_no
                    int customerId = GetCustomerIdByPhoneNumber(CustomerPhoneNumberTb.Text, CustomerFnameTb.Text, CustLnameTb.Text);
                    if (customerId == -1)
                    {
                        MessageBox.Show("Invalid customer information.");
                        return;
                    }

                    // Find product_id based on product_name
                    int productId = GetProductIdByName(productName);
                    if (productId == -1)
                    {
                        MessageBox.Show("Invalid product information.");
                        return;
                    }
                    // MessageBox.Show(""+id);
                    //MessageBox.Show("" + productId);

                    // Insert data into the sales table with the provided sales_id
                    string insertQuery = "INSERT INTO sales (sales_id, sales_quantity, sales_amount, sales_date, customer_id, product_id, input_by, input_dt) " +
                                        "VALUES (@SalesID, @Quantity, @Amount, @Date, @CustomerID, @ProductID, @InputBy, @InputDate)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        // Set parameters for the SQL command
                        insertCmd.Parameters.AddWithValue("@SalesID", id);
                        insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                        insertCmd.Parameters.AddWithValue("@Amount", amount);
                        insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        insertCmd.Parameters.AddWithValue("@CustomerID", customerId);
                        insertCmd.Parameters.AddWithValue("@ProductID", productId);
                        insertCmd.Parameters.AddWithValue("@InputBy", name); // Replace with the actual username or employee ID
                        insertCmd.Parameters.AddWithValue("@InputDate", DateTime.Now);

                        // Execute the query
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Bill saved successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the bill: " + ex.Message);
            }
        }
        private void ClearYourBillDGV()
        {
            // Clear the DataGridView
            YourBillDGV.Rows.Clear();
            CalculateGrandTotal(); // You may need to recalculate the grand total or perform other actions based on your requirements
        }

        private List<BillItem> GetItemsFromDataGridView()
        {
            List<BillItem> items = new List<BillItem>();

            foreach (DataGridViewRow row in YourBillDGV.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null && row.Cells[4].Value != null)
                {
                    BillItem item = new BillItem
                    {
                        ID = Convert.ToInt32(row.Cells[0].Value),
                        ProductName = row.Cells[1].Value.ToString(),
                        Quantity = Convert.ToInt32(row.Cells[2].Value),
                        Price = Convert.ToDecimal(row.Cells[3].Value),
                        Total = Convert.ToDecimal(row.Cells[4].Value)
                    };

                    items.Add(item);
                }
            }

            return items;
        }


        private void SaveBillBtn_Click(object sender, EventArgs e)
        {
            // Retrieve information from the DataGridView
            List<BillItem> items = GetItemsFromDataGridView();

            // Assuming you have a function to save the bill information to your database
            // Modify this part according to your actual database saving logic
            foreach (BillItem item in items)
            {
                SaveBill(item.ID, item.ProductName, item.Quantity, item.Total); // Include the salesID parameter
            }

            // Update the panel with the latest product information
            DisplayAllProductsOnPanel();

            // Update the DataGridView with the latest data
            DisplayDataOnDGV();
            ClearYourBillDGV();
            //DisplayDataOnDGV();
        }
        
        private void LoadReportData()
        {
            try
            {
                //string connectionString = "Your_Connection_String_Here"; // Replace with your actual connection string

                // Get the selected month from the ComboBox
                string selectedMonth = dropdown1.SelectedItem?.ToString();

                // If no month is selected, exit the method
                if (string.IsNullOrEmpty(selectedMonth))
                {
                    return;
                }

                // Convert month name to a number (1 to 12)
                int monthNumber = Array.IndexOf(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames, selectedMonth) + 1;

                string query = $"SELECT sales_id, sales_quantity, sales_amount, sales_date, customer_id, product_id, input_by, input_dt FROM sales " +
                               $"WHERE MONTH(sales_date) = {monthNumber}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Clear existing data in the DataGridView
                            reportdataGridView.Rows.Clear();

                            // Populate the DataGridView with the retrieved data
                            while (reader.Read())
                            {
                                // Add a breakpoint here and inspect the data during debugging
                                reportdataGridView.Rows.Add(
                                    reader["sales_id"],
                                    reader["sales_quantity"],
                                    reader["sales_amount"],
                                    reader["sales_date"],
                                    reader["customer_id"],
                                    reader["product_id"],
                                    reader["input_by"],
                                    reader["input_dt"]
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log or display an error message)
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
    

    private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteSelectedProduct();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            UpdateProduct();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(1);
            DisplayAllCustomersOnPanel();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(0);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(2);
          
        }

        private void label17_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(3);
            DisplayAllProductsOnPanel();
            DisplaySalesDataOnPanel();

        }

        private void label18_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(4);

            // Populate the ComboBox with months
            dropdown1.Items.AddRange(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames);
            reportdataGridView.Columns.Add("sales_id", "Sales ID");
            reportdataGridView.Columns.Add("sales_quantity", "Sales Quantity");
            reportdataGridView.Columns.Add("sales_amount", "Sales Amount");
            reportdataGridView.Columns.Add("sales_date", "Sales Date");
            reportdataGridView.Columns.Add("customer_id", "Customer ID");
            reportdataGridView.Columns.Add("product_id", "Product ID");
            reportdataGridView.Columns.Add("input_by", "Input By");
            reportdataGridView.Columns.Add("input_dt", "Input Date");

            // Call a method to load data into the DataGridView
            LoadReportData();

            

        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            UpdateProductQuantity();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void ProductNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            DeleteSelectedProduct();
        }

        private void bunifuTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddCustBtn_Click(object sender, EventArgs e)
        {
            InsertCustomer(CFNameTb.Text, CLNameTb.Text,PhoneTb.Text);
        }
        
        private void CatNameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddCatBtn_Click(object sender, EventArgs e)
        {
            AddNewCategory();
        }

        private void DeleteCatBtn_Click(object sender, EventArgs e)
        {
            DeleteCategory();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        

        private void dropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DelCustBtn_Click(object sender, EventArgs e)
        {
            DeleteSelectedCustomer();
        }

        private void EditCustBtn_Click(object sender, EventArgs e)
        {

        }

        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CustomersDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("" + e.ColumnIndex);
            int deleteColumnIndex = 4;
          

            // Add a breakpoint here by clicking in the left margin
            if (e.ColumnIndex == deleteColumnIndex && e.RowIndex >= 0)
            {
                int customerId = Convert.ToInt32(CustomersDGV.Rows[e.RowIndex].Cells["customer_id"].Value);
                HandleDeleteCustomer(customerId);
            }
        }

        private void BprodDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void PhoneTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshBillDGV();
        }

       
        private void BillingListDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DisplayDataOnDGV();
        }

        private void QuanTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void YourBillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportData();
        }
    }
}
